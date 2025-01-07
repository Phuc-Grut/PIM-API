using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductSpecificationCodeCommandHandler : CommandHandler, IRequestHandler<ProductSpecificationCodeAddCommand, ValidationResult>, IRequestHandler<ProductSpecificationCodeDeleteCommand, ValidationResult>, IRequestHandler<ProductSpecificationCodeEditCommand, ValidationResult>
    {
        private readonly IProductSpecificationCodeRepository _productSpecificationCodeRepository;

        public ProductSpecificationCodeCommandHandler(IProductSpecificationCodeRepository ProductSpecificationCodeRepository)
        {
            _productSpecificationCodeRepository = ProductSpecificationCodeRepository;
        }
        public void Dispose()
        {
            _productSpecificationCodeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductSpecificationCodeAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationCode
            {
                Id = request.Id,
                ProductId = request.ProductId,
                Name = request.Name,
                DuplicateAllowed = request.DuplicateAllowed,
                Status = request.Status,
                DataTypes = request.DataTypes,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productSpecificationCodeRepository.Add(productAttributeMapping);
            return await Commit(_productSpecificationCodeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductSpecificationCodeDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productSpecificationCodeRepository)) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationCode
            {
                Id = request.Id
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productSpecificationCodeRepository.Remove(productAttributeMapping);
            return await Commit(_productSpecificationCodeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductSpecificationCodeEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productAttributeMapping = new ProductSpecificationCode
            {
                Id = request.Id,
                ProductId = request.ProductId,
                Name = request.Name,
                DuplicateAllowed = request.DuplicateAllowed,
                Status = request.Status,
                DataTypes = request.DataTypes,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productAttributeMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productSpecificationCodeRepository.Update(productAttributeMapping);
            return await Commit(_productSpecificationCodeRepository.UnitOfWork);
        }
    }
}
