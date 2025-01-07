using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Application.PIM.Commands
{
    internal class StoreCommandHandler : CommandHandler, IRequestHandler<StoreAddCommand, ValidationResult>,
                                                        IRequestHandler<StoreDeleteCommand, ValidationResult>,
                                                        IRequestHandler<StoreEditCommand, ValidationResult>,
                                                        IRequestHandler<StoreSortCommand, ValidationResult>
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        protected readonly IPIMContextProcedures _storeProcedures;

        public StoreCommandHandler(IStoreRepository StoreRepository, IProductRepository productRepository, IPIMContextProcedures storeProcedures)
        {
            _storeRepository = StoreRepository;
            _productRepository = productRepository;
            _storeProcedures = storeProcedures;
        }
        public void Dispose()
        {
            _storeRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(StoreAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_storeRepository)) return request.ValidationResult;
            var store = new Store
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                Phone = request.Phone,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //store.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _storeRepository.Add(store);
            return await Commit(_storeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(StoreDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_storeRepository)) return request.ValidationResult;

            var products = await _storeProcedures.SP_GET_PRODUCT_BY_STOREIDAsync(request.Id);

            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var store = new Store
            {
                Id = request.Id
            };

            _storeRepository.Remove(store);
            return await Commit(_storeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(StoreEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_storeRepository)) return request.ValidationResult;
            var data = await _storeRepository.GetById(request.Id);
            var store = new Store
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                Phone = request.Phone,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                CreatedByName = data.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //store.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _storeRepository.Update(store);
            return await Commit(_storeRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(StoreSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _storeRepository.GetAll();

            List<Store> stores = new List<Store>();

            foreach (var sort in request.SortList)
            {
                Store store = data.FirstOrDefault(c => c.Id == sort.Id);
                if (store != null)
                {
                    store.DisplayOrder = sort.SortOrder;
                    stores.Add(store);
                }
            }
            _storeRepository.Update(stores);
            return await Commit(_storeRepository.UnitOfWork);
        }
    }
}
