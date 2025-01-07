using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductSpecificationAttributeMappingCommandHandler : CommandHandler, IRequestHandler<ProductSpecificationAttributeMappingAddCommand, ValidationResult>, IRequestHandler<ProductSpecificationAttributeMappingDeleteCommand, ValidationResult>, IRequestHandler<ProductSpecificationAttributeMappingEditCommand, ValidationResult>
    {
        private readonly IProductSpecificationAttributeMappingRepository _productAttributeMappingRepository;

        public ProductSpecificationAttributeMappingCommandHandler(IProductSpecificationAttributeMappingRepository ProductSpecificationAttributeMappingRepository)
        {
            _productAttributeMappingRepository = ProductSpecificationAttributeMappingRepository;
        }
        public void Dispose()
        {
            _productAttributeMappingRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductSpecificationAttributeMappingAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationAttributeMapping
            {
                Id = request.Id,
                ProductId = request.ProductId,
                SpecificationAttributeId = request.SpecificationAttributeId,
                SpecificationAttributeOptionId = request.SpecificationAttributeOptionId,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productAttributeMappingRepository.Add(productAttributeMapping);
            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductSpecificationAttributeMappingDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productAttributeMappingRepository)) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationAttributeMapping
            {
                Id = request.Id
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productAttributeMappingRepository.Remove(productAttributeMapping);
            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductSpecificationAttributeMappingEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationAttributeMapping
            {
                Id = request.Id,
                ProductId = request.ProductId,
                SpecificationAttributeId = request.SpecificationAttributeId,
                SpecificationAttributeOptionId = request.SpecificationAttributeOptionId,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productAttributeMappingRepository.Update(productAttributeMapping);
            return await Commit(_productAttributeMappingRepository.UnitOfWork);
        }
    }
}
