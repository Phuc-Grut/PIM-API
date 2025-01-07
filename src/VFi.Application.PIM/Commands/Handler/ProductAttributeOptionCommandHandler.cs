//using VFi.Domain.PIM.Interfaces;
//using VFi.Domain.PIM.Models;
//using VFi.Infra.PIM.Repository;
//using VFi.NetDevPack.Messaging;
//using FluentValidation.Results;
//using MediatR;

//namespace VFi.Application.PIM.Commands
//{
//    internal class ProductAttributeOptionCommandHandler : CommandHandler, IRequestHandler<ProductAttributeOptionAddCommand, ValidationResult>, IRequestHandler<ProductAttributeOptionDeleteCommand, ValidationResult>, IRequestHandler<ProductAttributeOptionEditCommand, ValidationResult>
//    {
//        private readonly IProductAttributeOptionRepository _deliveryTimeRepository;

//        public ProductAttributeOptionCommandHandler(IProductAttributeOptionRepository ProductAttributeOptionRepository)
//        {
//            _deliveryTimeRepository = ProductAttributeOptionRepository;
//        }
//        public void Dispose()
//        {
//            _deliveryTimeRepository.Dispose();
//        }

//        public async Task<ValidationResult> Handle(ProductAttributeOptionAddCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid()) return request.ValidationResult;
//            var deliveryTime = new ProductAttributeOption
//            {
//                Id = request.Id,
//                ProductAttributeId = request.ProductAttributeId,
//                Name = request.Name,
//                Alias = request.Alias,
//                MediaFileId = request.MediaFileId,
//                Color = request.Color,
//                PriceAdjustment = request.PriceAdjustment,
//                WeightAdjustment = request.WeightAdjustment,
//                IsPreSelected = request.IsPreSelected,
//                DisplayOrder = request.DisplayOrder,
//                ValueTypeId = request.ValueTypeId,
//                LinkedProductId = request.LinkedProductId,
//                Quantity = request.Quantity,
//                CreatedBy = request.CreatedBy,
//                CreatedDate = request.CreatedDate
//            };

//            //add domain event
//            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

//            _deliveryTimeRepository.Add(deliveryTime);
//            return await Commit(_deliveryTimeRepository.UnitOfWork);
//        }

//        public async Task<ValidationResult> Handle(ProductAttributeOptionDeleteCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
//            var deliveryTime = new ProductAttributeOption
//            {
//                Id = request.Id
//            };

//            //add domain event
//            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

//            _deliveryTimeRepository.Remove(deliveryTime);
//            return await Commit(_deliveryTimeRepository.UnitOfWork);
//        }

//        public async Task<ValidationResult> Handle(ProductAttributeOptionEditCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
//            ProductAttributeOption dataProductAttributeOption = await _deliveryTimeRepository.GetById(request.Id);

//            var deliveryTime = new ProductAttributeOption
//            {
//                Id = request.Id,
//                ProductAttributeId = request.ProductAttributeId,
//                Name = request.Name,
//                Alias = request.Alias,
//                MediaFileId = request.MediaFileId,
//                Color = request.Color,
//                PriceAdjustment = request.PriceAdjustment,
//                WeightAdjustment = request.WeightAdjustment,
//                IsPreSelected = request.IsPreSelected,
//                DisplayOrder = request.DisplayOrder,
//                ValueTypeId = request.ValueTypeId,
//                LinkedProductId = request.LinkedProductId,
//                Quantity = request.Quantity,
//                CreatedBy = dataProductAttributeOption.CreatedBy,
//                CreatedDate = dataProductAttributeOption.CreatedDate,
//                UpdatedBy = request.UpdatedBy,
//                UpdatedDate = request.UpdatedDate
//            };

//            //add domain event
//            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

//            _deliveryTimeRepository.Update(deliveryTime);
//            return await Commit(_deliveryTimeRepository.UnitOfWork);
//        }
//    }
//}
