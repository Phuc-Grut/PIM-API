using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using System.Linq;

namespace VFi.Application.PIM.Commands
{
    internal class SpecificationAttributeCommandHandler : CommandHandler, IRequestHandler<SpecificationAttributeAddCommand, ValidationResult>,
                                                                            IRequestHandler<SpecificationAttributeDeleteCommand, ValidationResult>,
                                                                            IRequestHandler<SpecificationAttributeEditCommand, ValidationResult>,
                                                                            IRequestHandler<SpecificationAttributeSortCommand, ValidationResult>
    {
        private readonly ISpecificationAttributeRepository _attributeRepository;
        private readonly ISpecificationAttributeOptionRepository _optionRepository;
        private readonly IProductSpecificationAttributeMappingRepository _productSpecificationAttributeMappingRepository;

        public SpecificationAttributeCommandHandler(ISpecificationAttributeRepository AttributeRepository, ISpecificationAttributeOptionRepository OptionRepository, IProductSpecificationAttributeMappingRepository productSpecificationAttributeMappingRepository)
        {
            _attributeRepository = AttributeRepository;
            _optionRepository = OptionRepository;
            _productSpecificationAttributeMappingRepository = productSpecificationAttributeMappingRepository;
        }
        public void Dispose()
        {
            _attributeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(SpecificationAttributeAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var attribute = new SpecificationAttribute
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Alias = request.Alias,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedDate = request.CreatedDate,
                CreatedBy = request.CreatedBy,
                CreatedByName = request.CreatedByName,
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _attributeRepository.Add(attribute);
            var result = await Commit(_attributeRepository.UnitOfWork);
            if (!result.IsValid) return result;

            List<SpecificationAttributeOption> list = new List<SpecificationAttributeOption>();
            if (request.Option?.Count > 0)
            {
                var i = 1;
                foreach (var u in request.Option)
                {
                    list.Add(new SpecificationAttributeOption()
                    {
                        Id = Guid.NewGuid(),
                        SpecificationAttributeId = request.Id,
                        Name = u.Name,
                        Code = u.Code,
                        NumberValue = u.NumberValue,
                        Color = u.Color,
                        DisplayOrder = i,
                        CreatedDate = u.CreatedDate,
                        CreatedBy = u.CreatedBy
                    });
                    i++;
                }
                _optionRepository.Add(list);
                await CommitNoCheck(_optionRepository.UnitOfWork);
            }
            return result;
        }

        public async Task<ValidationResult> Handle(SpecificationAttributeDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_attributeRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "specificationAttributeId", request.Id } };

            var products = await _productSpecificationAttributeMappingRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var attribute = new SpecificationAttribute
            {
                Id = request.Id
            };

            _attributeRepository.Remove(attribute);
            return await Commit(_attributeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(SpecificationAttributeEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_attributeRepository)) return request.ValidationResult;
            var attribute = new SpecificationAttribute
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Alias = request.Alias,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedDate = request.CreatedDate,
                UpdatedDate = request.UpdatedDate,
                CreatedBy = request.CreatedBy,
                UpdatedBy = request.UpdatedBy,
                CreatedByName = request.CreatedByName,
                UpdatedByName = request.UpdatedByName,
            };
            _attributeRepository.Update(attribute);
            var result = await Commit(_attributeRepository.UnitOfWork);
            if (!result.IsValid) return result;

            List<SpecificationAttributeOption> list = new List<SpecificationAttributeOption>();
            List<SpecificationAttributeOption> listUpdate = new List<SpecificationAttributeOption>();
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
                        listUpdate.Add(new SpecificationAttributeOption()
                        {
                            Id = (Guid)u.Id,
                            SpecificationAttributeId = request.Id,
                            Name = u.Name,
                            Code = u.Code,
                            NumberValue = u.NumberValue,
                            Color = u.Color,
                            DisplayOrder = i,
                            CreatedDate = u.CreatedDate,
                            UpdatedDate = u.UpdatedDate,
                            CreatedBy = u.CreatedBy,
                            UpdatedBy = u.UpdatedBy
                        });
                    }
                    else
                    {
                        list.Add(new SpecificationAttributeOption()
                        {
                            Id = Guid.NewGuid(),
                            SpecificationAttributeId = request.Id,
                            Name = u.Name,
                            Code = u.Code,
                            NumberValue = u.NumberValue,
                            Color = u.Color,
                            DisplayOrder = i,
                            CreatedDate = u.CreatedDate,
                            UpdatedDate = u.UpdatedDate,
                            CreatedBy = u.CreatedBy,
                            UpdatedBy = u.UpdatedBy
                        });
                    }
                    i++;
                }

                _optionRepository.Add(list);
                _optionRepository.Update(listUpdate);
                _optionRepository.Remove(listDel);
                await CommitNoCheck(_optionRepository.UnitOfWork);
            }
            return result;
        }

        public async Task<ValidationResult> Handle(SpecificationAttributeSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _attributeRepository.GetAll();

            List<SpecificationAttribute> list = new List<SpecificationAttribute>();

            foreach (var sort in request.SortList)
            {
                SpecificationAttribute obj = data.FirstOrDefault(c => c.Id == sort.Id);
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
