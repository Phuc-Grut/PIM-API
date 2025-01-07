using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class WarehouseCommandHandler : CommandHandler, IRequestHandler<WarehouseAddCommand, ValidationResult>,
                                                            IRequestHandler<WarehouseDeleteCommand, ValidationResult>,
                                                            IRequestHandler<WarehouseEditCommand, ValidationResult>,
                                                            IRequestHandler<WarehouseSortCommand, ValidationResult>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseCommandHandler(IWarehouseRepository WarehouseRepository)
        {
            _warehouseRepository = WarehouseRepository;
        }
        public void Dispose()
        {
            _warehouseRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(WarehouseAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_warehouseRepository)) return request.ValidationResult;
            var warehouse = new Warehouse
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Token = request.Token,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Ward = request.Ward,
                District = request.District,
                Company = request.Company,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Province = request.Province,
                Api = request.Api,
                DisplayOrder = request.DisplayOrder,
                Status = request.Status,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName,
            };

            //add domain event
            //warehouse.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _warehouseRepository.Add(warehouse);
            return await Commit(_warehouseRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(WarehouseDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_warehouseRepository)) return request.ValidationResult;
            var warehouse = new Warehouse
            {
                Id = request.Id
            };

            //add domain event
            //warehouse.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));
            try
            {
                _warehouseRepository.Remove(warehouse);
                return await Commit(_warehouseRepository.UnitOfWork);
            } catch (Exception ex)
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }
        }

        public async Task<ValidationResult> Handle(WarehouseEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_warehouseRepository)) return request.ValidationResult;
            var data = await _warehouseRepository.GetById(request.Id);
            var warehouse = new Warehouse
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Token = request.Token,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Ward = request.Ward,
                District = request.District,
                Company = request.Company,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Province = request.Province,
                Api = request.Api,
                DisplayOrder = request.DisplayOrder,
                Status = request.Status,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                CreatedByName = data.CreatedByName,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //warehouse.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _warehouseRepository.Update(warehouse);
            return await Commit(_warehouseRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(WarehouseSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _warehouseRepository.GetAll();

            List<Warehouse> warehouses = new List<Warehouse>();

            foreach (var sort in request.SortList)
            {
                Warehouse warehouse = data.FirstOrDefault(c => c.Id == sort.Id);
                if (warehouse != null)
                {
                    warehouse.DisplayOrder = sort.SortOrder;
                    warehouses.Add(warehouse);
                }
            }
            _warehouseRepository.Update(warehouses);
            return await Commit(_warehouseRepository.UnitOfWork);
        }
    }
}
