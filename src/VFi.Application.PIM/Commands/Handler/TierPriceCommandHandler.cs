using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class TierPriceCommandHandler : CommandHandler, IRequestHandler<TierPriceAddCommand, ValidationResult>, IRequestHandler<TierPriceDeleteCommand, ValidationResult>, IRequestHandler<TierPriceEditCommand, ValidationResult>
    {
        private readonly ITierPriceRepository _tierPriceRepository;

        public TierPriceCommandHandler(ITierPriceRepository TierPriceRepository)
        {
            _tierPriceRepository = TierPriceRepository;
        }
        public void Dispose()
        {
            _tierPriceRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(TierPriceAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var tierPrice = new TierPrice
            {
                Id = request.Id,
                Price = request.Price,
                CalculationMethod= request.CalculationMethod,
                CreatedBy= request.CreatedBy,
                CreatedDate= request.CreatedDate,
                EndDate= request.EndDate,
                ProductId= request.ProductId,
                Quantity= request.Quantity,
                StartDate = request.StartDate,
                StoreId= request.StoreId
            };

            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _tierPriceRepository.Add(tierPrice);
            return await Commit(_tierPriceRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(TierPriceDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_tierPriceRepository)) return request.ValidationResult;
            var tierPrice = new TierPrice
            {
                Id = request.Id
            };

            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _tierPriceRepository.Remove(tierPrice);
            return await Commit(_tierPriceRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(TierPriceEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            TierPrice dataTierPrice = await _tierPriceRepository.GetById(request.Id);
            var tierPrice = new TierPrice
            {
                Id = request.Id,
                Price = request.Price,
                CalculationMethod = request.CalculationMethod,
                EndDate = request.EndDate,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                StartDate = request.StartDate,
                StoreId = request.StoreId,
                CreatedBy = dataTierPrice.CreatedBy,
                CreatedDate = dataTierPrice.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _tierPriceRepository.Update(tierPrice);
            return await Commit(_tierPriceRepository.UnitOfWork);
        }
    }
}
