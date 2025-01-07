using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;

namespace VFi.Application.PIM.Commands
{
    internal class ProductAttributeMappingCommandHandler : CommandHandler, IRequestHandler<ProductAttributeMappingAddCommand, ValidationResult>, IRequestHandler<ProductAttributeMappingDeleteCommand, ValidationResult>, IRequestHandler<ProductAttributeMappingEditCommand, ValidationResult>
    {
        private readonly IContextUser _context;
        private readonly IProductProductAttributeMappingRepository _productAttributeMappingRepository;
        private readonly IProductVariantAttributeValueRepository _ProductVariantAttributeValueRepository;

        public ProductAttributeMappingCommandHandler(IProductProductAttributeMappingRepository productAttributeMappingRepository, IContextUser context, IProductVariantAttributeValueRepository productVariantAttributeValueRepository)
        {
            _productAttributeMappingRepository = productAttributeMappingRepository;
            _context = context;
            _ProductVariantAttributeValueRepository=productVariantAttributeValueRepository;
        }
        public void Dispose()
        {
            _productAttributeMappingRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductAttributeMappingAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductProductAttributeMapping
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ProductAttributeId = request.ProductAttributeId,
                TextPrompt = request.TextPrompt,
                CustomData = request.CustomData,
                IsRequired = request.IsRequired,
                AttributeControlTypeId = request.AttributeControlTypeId,
                DisplayOrder = request.DisplayOrder
            };
            _productAttributeMappingRepository.Add(productAttributeMapping);

            if(request?.ListDetail.Count > 0)
            {
                var i = 1;
                List<ProductVariantAttributeValue> list = new List<ProductVariantAttributeValue>();
                foreach (var u in request?.ListDetail)
                {
                    list.Add(new ProductVariantAttributeValue()
                    {
                        Id = Guid.NewGuid(),
                        ProductVariantAttributeId = request.Id,
                        Code = u.Code,
                        Name = u.Name,
                        Alias = u.Alias,
                        Color = u.Color,
                        DisplayOrder = i,
                        PriceAdjustment= u.PriceAdjustment,
                        WeightAdjustment= u.WeightAdjustment,
                        CreatedDate =  DateTime.Now,
                        CreatedBy = _context.GetUserId(),
                        CreatedByName= _context.UserName
                    });
                    i++;
                }
                _ProductVariantAttributeValueRepository.Add(list);
            }

            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductAttributeMappingDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productAttributeMappingRepository)) return request.ValidationResult;
            var productAttributeMapping = new ProductProductAttributeMapping
            {
                Id = request.Id
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productAttributeMappingRepository.Remove(productAttributeMapping);
            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductAttributeMappingEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductProductAttributeMapping
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ProductAttributeId = request.ProductAttributeId,
                TextPrompt = request.TextPrompt,
                CustomData = request.CustomData,
                IsRequired = request.IsRequired,
                AttributeControlTypeId = request.AttributeControlTypeId,
                DisplayOrder = request.DisplayOrder
            };

            if (request?.ListDetail.Count > 0)
            {
                var i = 1;
                List<ProductVariantAttributeValue> addInv = new List<ProductVariantAttributeValue>();
                List<ProductVariantAttributeValue> editInv = new List<ProductVariantAttributeValue>();
                var listInv = await _ProductVariantAttributeValueRepository.GetByParentId(request.Id);
                var listid = request?.ListDetail.Select(x => x.Id).ToArray();
                var delInv = listInv.Where(x => !listid.Contains(x.Id));
                foreach (var u in request?.ListDetail)
                {
                    if (u.Id != null && u.Id != new Guid())
                    {
                        editInv.Add(new ProductVariantAttributeValue()
                        {
                            Id = (Guid)u.Id,
                            ProductVariantAttributeId = request.Id,
                            Code = u.Code,
                            Name = u.Name,
                            Alias = u.Alias,
                            Color = u.Color,
                            DisplayOrder = i,
                            PriceAdjustment= u.PriceAdjustment,
                            WeightAdjustment= u.WeightAdjustment,
                            CreatedDate =  DateTime.Now,
                            CreatedBy = _context.GetUserId(),
                            CreatedByName= _context.UserName
                        });
                        i++;
                    } else
                    {
                        addInv.Add(new ProductVariantAttributeValue()
                        {
                            Id = Guid.NewGuid(),
                            ProductVariantAttributeId = request.Id,
                            Code = u.Code,
                            Name = u.Name,
                            Alias = u.Alias,
                            Color = u.Color,
                            DisplayOrder = i,
                            PriceAdjustment= u.PriceAdjustment,
                            WeightAdjustment= u.WeightAdjustment,
                            CreatedDate =  DateTime.Now,
                            CreatedBy = _context.GetUserId(),
                            CreatedByName= _context.UserName
                        });
                        i++;
                    }
                    
                }
                _ProductVariantAttributeValueRepository.Add(addInv);
                _ProductVariantAttributeValueRepository.Update(editInv);
                _ProductVariantAttributeValueRepository.Remove(delInv);
                await CommitNoCheck(_ProductVariantAttributeValueRepository.UnitOfWork);
            }
            else
            {
                var listInv = await _ProductVariantAttributeValueRepository.GetByParentId(request.Id);
                _ProductVariantAttributeValueRepository.Remove(listInv); 
                await CommitNoCheck(_ProductVariantAttributeValueRepository.UnitOfWork);
            }
            _productAttributeMappingRepository.Update(productAttributeMapping);
            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }
    }
}
