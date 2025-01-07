//using VFi.Domain.PIM.Interfaces;
//using VFi.Domain.PIM.Models;
//using VFi.Infra.PIM.Repository;
//using VFi.NetDevPack.Messaging;
//using FluentValidation.Results;
//using MediatR;

//namespace VFi.Application.PIM.Commands
//{
//    internal class SpecificationAttributeOptionCommandHandler : CommandHandler, IRequestHandler<SpecificationAttributeOptionAddCommand, ValidationResult>, IRequestHandler<SpecificationAttributeOptionDeleteCommand, ValidationResult>, IRequestHandler<SpecificationAttributeOptionEditCommand, ValidationResult>
//    {
//        private readonly ISpecificationAttributeOptionRepository _deliveryTimeRepository;

//        public SpecificationAttributeOptionCommandHandler(ISpecificationAttributeOptionRepository SpecificationAttributeOptionRepository)
//        {
//            _deliveryTimeRepository = SpecificationAttributeOptionRepository;
//        }
//        public void Dispose()
//        {
//            _deliveryTimeRepository.Dispose();
//        }

//        public async Task<ValidationResult> Handle(SpecificationAttributeOptionAddCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid()) return request.ValidationResult;
//            var deliveryTime = new SpecificationAttributeOption
//            {
//                Id = request.Id,
//                Name = request.Name,
//                Color= request.Color,
//                SpecificationAttributeId= request.SpecificationAttributeId,
//                NumberValue= request.NumberValue,
//                MediaFileId= request.MediaFileId,Alias= request.Alias,
//                DisplayOrder = request.DisplayOrder,
//                CreatedBy = request.CreatedBy,
//                CreatedDate = request.CreatedDate
//            };

//            //add domain event
//            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

//            _deliveryTimeRepository.Add(deliveryTime);
//            return await Commit(_deliveryTimeRepository.UnitOfWork);
//        }

//        public async Task<ValidationResult> Handle(SpecificationAttributeOptionDeleteCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
//            var deliveryTime = new SpecificationAttributeOption
//            {
//                Id = request.Id
//            };

//            //add domain event
//            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

//            _deliveryTimeRepository.Remove(deliveryTime);
//            return await Commit(_deliveryTimeRepository.UnitOfWork);
//        }

//        public async Task<ValidationResult> Handle(SpecificationAttributeOptionEditCommand request, CancellationToken cancellationToken)
//        {
//            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
//            SpecificationAttributeOption dataSpecificationAttributeOption = await _deliveryTimeRepository.GetById(request.Id);

//            var deliveryTime = new SpecificationAttributeOption
//            {
//                Id = request.Id,
//                Name = request.Name,
//                Color = request.Color,
//                SpecificationAttributeId = request.SpecificationAttributeId,
//                NumberValue = request.NumberValue,
//                MediaFileId = request.MediaFileId,
//                Alias = request.Alias,
//                DisplayOrder = request.DisplayOrder,
//                CreatedBy = dataSpecificationAttributeOption.CreatedBy,
//                CreatedDate = dataSpecificationAttributeOption.CreatedDate,
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
