using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductVariantAttributeCombinationCommandHandler : CommandHandler, IRequestHandler<ProductVariantAttributeCombinationAddCommand, ValidationResult>, IRequestHandler<ProductVariantAttributeCombinationDeleteCommand, ValidationResult>, IRequestHandler<ProductVariantAttributeCombinationEditCommand, ValidationResult>
    {
        private readonly IProductVariantAttributeCombinationRepository _productVariantAttributeCombinationRepository;

        public ProductVariantAttributeCombinationCommandHandler(IProductVariantAttributeCombinationRepository ProductVariantAttributeCombinationRepository)
        {
            _productVariantAttributeCombinationRepository = ProductVariantAttributeCombinationRepository;
        }
        public void Dispose()
        {
            _productVariantAttributeCombinationRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeCombinationAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productVariantAttributeCombination = new ProductVariantAttributeCombination
            {
                Id = request.Id,
                Name= request.Name,
                ProductId = request.ProductId,
                Sku = request.Sku,
                Gtin = request.Gtin,
                ManufacturerPartNumber = request.ManufacturerPartNumber,
                Price = request.Price,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                BasePriceAmount = request.BasePriceAmount,
                BasePriceBaseAmount = request.BasePriceBaseAmount,
                AssignedMediaFileIds = request.AssignedMediaFileIds,
                IsActive= request.IsActive,
                DeliveryTimeId = request.DeliveryTimeId,
                QuantityUnitId = request.QuantityUnitId,
                AttributesXml = request.AttributesXml,
                StockQuantity = request.StockQuantity,
                AllowOutOfStockOrders = request.AllowOutOfStockOrders,
                CreatedDate = request.CreatedDate,
                CreatedBy=request.CreatedBy,
            };

            //add domain event
            //productVariantAttributeCombination.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productVariantAttributeCombinationRepository.Add(productVariantAttributeCombination);
            return await Commit(_productVariantAttributeCombinationRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeCombinationDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productVariantAttributeCombinationRepository)) return request.ValidationResult;
            var productVariantAttributeCombination = new ProductVariantAttributeCombination
            {
                Id = request.Id
            };

            //add domain event
            //productVariantAttributeCombination.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productVariantAttributeCombinationRepository.Remove(productVariantAttributeCombination);
            return await Commit(_productVariantAttributeCombinationRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeCombinationEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var data = await _productVariantAttributeCombinationRepository.GetById(request.Id);
            var productVariantAttributeCombination = new ProductVariantAttributeCombination
            {
                Id = request.Id,
                Name = request.Name,
                ProductId = request.ProductId,
                Sku = request.Sku,
                Gtin = request.Gtin,
                ManufacturerPartNumber = request.ManufacturerPartNumber,
                Price = request.Price,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                BasePriceAmount = request.BasePriceAmount,
                BasePriceBaseAmount = request.BasePriceBaseAmount,
                AssignedMediaFileIds = request.AssignedMediaFileIds,
                IsActive = request.IsActive,
                DeliveryTimeId = request.DeliveryTimeId,
                QuantityUnitId = request.QuantityUnitId,
                AttributesXml = request.AttributesXml,
                StockQuantity = request.StockQuantity,
                AllowOutOfStockOrders = request.AllowOutOfStockOrders,
                CreatedBy = data.CreatedBy,
                CreatedDate= data.CreatedDate,
                UpdatedBy= request.UpdatedBy,
                UpdatedDate= request.UpdatedDate,
            };

            //add domain event
            //productVariantAttributeCombination.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productVariantAttributeCombinationRepository.Update(productVariantAttributeCombination);
            return await Commit(_productVariantAttributeCombinationRepository.UnitOfWork);
        }
    }
}
