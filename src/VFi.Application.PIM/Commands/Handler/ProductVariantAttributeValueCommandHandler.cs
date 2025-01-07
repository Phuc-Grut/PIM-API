using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductVariantAttributeValueCommandHandler : CommandHandler, IRequestHandler<ProductVariantAttributeValueAddCommand, ValidationResult>, IRequestHandler<ProductVariantAttributeValueDeleteCommand, ValidationResult>, IRequestHandler<ProductVariantAttributeValueEditCommand, ValidationResult>
    {
        private readonly IProductVariantAttributeValueRepository _deliveryTimeRepository;

        public ProductVariantAttributeValueCommandHandler(IProductVariantAttributeValueRepository ProductVariantAttributeValueRepository)
        {
            _deliveryTimeRepository = ProductVariantAttributeValueRepository;
        }
        public void Dispose()
        {
            _deliveryTimeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeValueAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var deliveryTime = new ProductVariantAttributeValue
            {
                Id = request.Id,
                ProductVariantAttributeId = request.ProductVariantAttributeId,
                Code = request.Code,
                Name = request.Name,
                Alias = request.Alias,
                Image = request.Image,
                Color = request.Color,
                PriceAdjustment = request.PriceAdjustment,
                WeightAdjustment = request.WeightAdjustment,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _deliveryTimeRepository.Add(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeValueDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
            var deliveryTime = new ProductVariantAttributeValue
            {
                Id = request.Id
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _deliveryTimeRepository.Remove(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductVariantAttributeValueEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
            ProductVariantAttributeValue dataProductVariantAttributeValue = await _deliveryTimeRepository.GetById(request.Id);

            var deliveryTime = new ProductVariantAttributeValue
            {
                Id = request.Id,
                ProductVariantAttributeId = request.ProductVariantAttributeId,
                Code = request.Code,
                Name = request.Name,
                Alias = request.Alias,
                Image = request.Image,
                Color = request.Color,
                PriceAdjustment = request.PriceAdjustment,
                WeightAdjustment = request.WeightAdjustment,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = dataProductVariantAttributeValue.CreatedBy,
                CreatedDate = dataProductVariantAttributeValue.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _deliveryTimeRepository.Update(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }
    }
}
