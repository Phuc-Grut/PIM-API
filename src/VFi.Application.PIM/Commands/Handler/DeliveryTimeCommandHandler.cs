using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class DeliveryTimeCommandHandler : CommandHandler, IRequestHandler<DeliveryTimeAddCommand, ValidationResult>,
                                                                IRequestHandler<DeliveryTimeDeleteCommand, ValidationResult>,
                                                                IRequestHandler<DeliveryTimeEditCommand, ValidationResult>,
                                                                IRequestHandler<DeliveryTimeSortCommand, ValidationResult>
    {
        private readonly IDeliveryTimeRepository _deliveryTimeRepository;
        private readonly IProductRepository _productRepository;

        public DeliveryTimeCommandHandler(IDeliveryTimeRepository DeliveryTimeRepository, IProductRepository productRepository)
        {
            _deliveryTimeRepository = DeliveryTimeRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _deliveryTimeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(DeliveryTimeAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var deliveryTime = new DeliveryTime
            {
                Id = request.Id,
                Name = request.Name,
                IsDefault = request.IsDefault,
                MinDays = request.MinDays,
                MaxDays = request.MaxDays,
                DisplayOrder = request.DisplayOrder,
                Status = request.Status,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate.Value,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _deliveryTimeRepository.Add(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(DeliveryTimeDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "deliveryTimeId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var deliveryTime = new DeliveryTime
            {
                Id = request.Id
            };

            _deliveryTimeRepository.Remove(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(DeliveryTimeEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_deliveryTimeRepository)) return request.ValidationResult;
            DeliveryTime dataDeliveryTime = await _deliveryTimeRepository.GetById(request.Id);

            var deliveryTime = new DeliveryTime
            {
                Id = request.Id,
                Name = request.Name,
                IsDefault = request.IsDefault,
                MinDays = request.MinDays,
                MaxDays = request.MaxDays,
                DisplayOrder = request.DisplayOrder,
                Status = request.Status,    
                CreatedBy = dataDeliveryTime.CreatedBy,
                CreatedDate = dataDeliveryTime.CreatedDate,
                CreatedByName = dataDeliveryTime.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //deliveryTime.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _deliveryTimeRepository.Update(deliveryTime);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(DeliveryTimeSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _deliveryTimeRepository.GetAll();

            List<DeliveryTime> list = new List<DeliveryTime>();

            foreach (var sort in request.SortList)
            {
                DeliveryTime obj = data.FirstOrDefault(c => c.Id == sort.Id);
                if (obj != null)
                {
                    obj.DisplayOrder = sort.SortOrder;
                    list.Add(obj);
                }
            }
            _deliveryTimeRepository.Update(list);
            return await Commit(_deliveryTimeRepository.UnitOfWork);
        }
    }
}
