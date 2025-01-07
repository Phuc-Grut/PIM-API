using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.PIM.Commands
{
    internal class ProductCategoryMappingCommandHandler : CommandHandler, IRequestHandler<ProductCategoryMappingAddCommand, ValidationResult>, IRequestHandler<ProductCategoryMappingDeleteCommand, ValidationResult>, IRequestHandler<ProductCategoryMappingEditCommand, ValidationResult>
    {
        private readonly IProductCategoryMappingRepository _productCategoryMappingRepository;

        public ProductCategoryMappingCommandHandler(IProductCategoryMappingRepository ProductCategoryMappingRepository)
        {
            _productCategoryMappingRepository = ProductCategoryMappingRepository;
        }
        public void Dispose()
        {
            _productCategoryMappingRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductCategoryMappingAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productCategoryMapping = new ProductCategoryMapping
            {
                Id = request.Id,
                ProductId= request.ProductId,
                CategoryId= request.CategoryId,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productCategoryMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productCategoryMappingRepository.Add(productCategoryMapping);
            return await Commit(_productCategoryMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductCategoryMappingDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productCategoryMappingRepository)) return request.ValidationResult;
            var productCategoryMapping = new ProductCategoryMapping
            {
                Id = request.Id
            };

            //add domain event
            //productCategoryMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productCategoryMappingRepository.Remove(productCategoryMapping);
            return await Commit(_productCategoryMappingRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductCategoryMappingEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productCategoryMapping = new ProductCategoryMapping
            {
                Id = request.Id,
                ProductId = request.ProductId,
                CategoryId = request.CategoryId,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //productCategoryMapping.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productCategoryMappingRepository.Update(productCategoryMapping);
            return await Commit(_productCategoryMappingRepository.UnitOfWork);
        }
       
    }
}
