using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductAttributeCommandHandler : CommandHandler, IRequestHandler<ProductAttributeAddCommand,ValidationResult>,
                                                                    IRequestHandler<ProductAttributeDeleteCommand, ValidationResult>,
                                                                    IRequestHandler<ProductAttributeEditCommand, ValidationResult>,
                                                                    IRequestHandler<ProductAttributeSortCommand, ValidationResult>
    {
        private readonly IProductAttributeRepository _attributeRepository;
        private readonly IProductAttributeOptionRepository _optionRepository;
        private readonly IProductProductAttributeMappingRepository _productProductAttributeMappingRepository;

        public ProductAttributeCommandHandler(IProductAttributeRepository AttributeRepository, IProductAttributeOptionRepository OptionRepository, IProductProductAttributeMappingRepository productProductAttributeMappingRepository)
        {
            _attributeRepository = AttributeRepository;
            _optionRepository = OptionRepository;
            _productProductAttributeMappingRepository = productProductAttributeMappingRepository;
        }
        public void Dispose()
        {
            _attributeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductAttributeAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var attribute = new ProductAttribute
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Alias= request.Alias,
                AllowFiltering= request.AllowFiltering,
                SearchType = request.SearchType,
                IsOption = request.IsOption,
                DisplayOrder = request.DisplayOrder,
                Mapping = request.Mapping,
                CreatedDate = request.CreatedDate,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByName,
                Status = request.Status
            };
            _attributeRepository.Add(attribute);
            var result = await Commit(_attributeRepository.UnitOfWork);
            if (!result.IsValid) return result;
            List<ProductAttributeOption> list = new List<ProductAttributeOption>();
            if (request.Option?.Count > 0)
            {
                var i = 1;
                foreach (var u in request.Option)
                {
                    list.Add(new ProductAttributeOption()
                    {
                        Id = Guid.NewGuid(),
                        ProductAttributeId = request.Id,
                        Name = u.Name,
                        Alias = u.Alias,
                        Image = u.Image,
                        Color = u.Color,
                        PriceAdjustment = u.PriceAdjustment,
                        WeightAdjustment = u.WeightAdjustment,
                        IsPreSelected = u.IsPreSelected,
                        DisplayOrder = i,
                        ValueTypeId = u.ValueTypeId,
                        LinkedProductId = u.LinkedProductId,
                        Quantity = u.Quantity,
                        CreatedDate = u.CreatedDate,
                        CreatedBy = u.CreatedBy
                    });
                    i++;
                }
                _optionRepository.Add(list);
                _ = await CommitNoCheck(_optionRepository.UnitOfWork);
            }
            return result;
        }

        public async Task<ValidationResult> Handle(ProductAttributeDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_attributeRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "productAttributeId", request.Id } };

            var products = await _productProductAttributeMappingRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var attribute = new ProductAttribute
            {
                Id = request.Id
            };
           
            _attributeRepository.Remove(attribute);
            return await Commit(_attributeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductAttributeEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_attributeRepository)) return request.ValidationResult;
            ProductAttribute dataProductAttribute = await _attributeRepository.GetById(request.Id);

            var attribute = new ProductAttribute
            {
                Id = request.Id,
                Code= request.Code,
                Name = request.Name,
                Description = request.Description,
                Alias = request.Alias,
                AllowFiltering = request.AllowFiltering,
                SearchType = request.SearchType,
                IsOption = request.IsOption,
                DisplayOrder = request.DisplayOrder,
                Mapping = request.Mapping,
                CreatedDate = dataProductAttribute.CreatedDate,
                UpdatedDate = request.UpdatedDate,
                CreatedBy = dataProductAttribute.CreatedBy,
                CreatedByName = dataProductAttribute.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedByName = request.UpdatedByName,
                Status = request.Status
            };
            _attributeRepository.Update(attribute);
            var result = await Commit(_attributeRepository.UnitOfWork);
            if (!result.IsValid) return result;

            List<ProductAttributeOption> listAdd = new List<ProductAttributeOption>();
            List<ProductAttributeOption> listUpdate = new List<ProductAttributeOption>();
            var listOption = await _optionRepository.GetByParentId(request.Id);
            var listOptionId = request.Option?.Select(x => x.Id).ToList();
            var listDel = listOption.Where(x => !listOptionId.Contains(x.Id)).ToList();

            if (request.Option is not null)
            {
                var i = 1;
                foreach (var u in request.Option)
                {
                    if (u.Id != null)
                    {
                        listUpdate.Add(new ProductAttributeOption()
                        {
                            Id = (Guid)u.Id,
                            ProductAttributeId = request.Id,
                            Name = u.Name,
                            Alias = u.Alias,
                            Image = u.Image,
                            Color = u.Color,
                            PriceAdjustment = u.PriceAdjustment,
                            WeightAdjustment = u.WeightAdjustment,
                            IsPreSelected = u.IsPreSelected,
                            DisplayOrder = i,
                            ValueTypeId = u.ValueTypeId,
                            LinkedProductId = u.LinkedProductId,
                            Quantity = u.Quantity,
                            CreatedDate = u.CreatedDate,
                            UpdatedDate = u.UpdatedDate,
                            CreatedBy = u.CreatedBy,
                            UpdatedBy = u.UpdatedBy
                        });
                    }
                    else
                    {
                        listAdd.Add(new ProductAttributeOption()
                        {
                            Id = Guid.NewGuid(),
                            ProductAttributeId = request.Id,
                            Name = u.Name,
                            Alias = u.Alias,
                            Image = u.Image,
                            Color = u.Color,
                            PriceAdjustment = u.PriceAdjustment,
                            WeightAdjustment = u.WeightAdjustment,
                            IsPreSelected = u.IsPreSelected,
                            DisplayOrder = i,
                            ValueTypeId = u.ValueTypeId,
                            LinkedProductId = u.LinkedProductId,
                            Quantity = u.Quantity,
                            CreatedDate = u.CreatedDate,
                            UpdatedDate = u.UpdatedDate,
                            CreatedBy = u.CreatedBy,
                            UpdatedBy = u.UpdatedBy
                        });
                    }
                    i++;
                }

                _optionRepository.Add(listAdd);
                _optionRepository.Update(listUpdate);
                _optionRepository.Remove(listDel);
                await CommitNoCheck(_optionRepository.UnitOfWork);
            }
            return result;
        }

        public async Task<ValidationResult> Handle(ProductAttributeSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _attributeRepository.GetAll();

            List<ProductAttribute> list = new List<ProductAttribute>();

            foreach (var sort in request.SortList)
            {
                ProductAttribute obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _attributeRepository.Update(list);
            return await Commit(_attributeRepository.UnitOfWork);
        }
    }
}
