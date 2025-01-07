using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductInventoryCommandHandler : CommandHandler, IRequestHandler<ProductInventoryAddCommand, ValidationResult>,
        IRequestHandler<ProductInventoryAddListCommand, ValidationResult>,
        IRequestHandler<ProductInventoryDeleteCommand, ValidationResult>, IRequestHandler<ProductInventoryEditCommand, ValidationResult>
    {
        private readonly IProductInventoryRepository _productInventoryRepository;

        public ProductInventoryCommandHandler(IProductInventoryRepository ProductInventoryRepository)
        {
            _productInventoryRepository = ProductInventoryRepository;
        }
        public void Dispose()
        {
            _productInventoryRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductInventoryAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productInventory = new ProductInventory
            {
                Id = request.Id,
                ProductId= request.ProductId,
                CreatedBy = request.CreatedBy,
                CreatedDate=request.CreatedDate,
                PlannedQuantity= request.PlannedQuantity,
                ReservedQuantity= request.ReservedQuantity,
                StockQuantity= request.StockQuantity,
                WarehouseId= request.WarehouseId
            };

            //add domain event
            //productInventory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productInventoryRepository.Add(productInventory);
            return await Commit(_productInventoryRepository.UnitOfWork);
        }
        public async Task<ValidationResult> Handle(ProductInventoryAddListCommand request, CancellationToken cancellationToken)
        {
            List<ProductInventory> list = new List<ProductInventory>();
            var dataCategory = await _productInventoryRepository.Filter((Guid)request.ProductId);
            _productInventoryRepository.Remove(dataCategory);
            if (request.ListInventory?.Count > 0)
            {
                foreach (var item in request.ListInventory)
                {
                    var productInventory = new ProductInventory
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        PlannedQuantity = item.PlannedQuantity,
                        ReservedQuantity = item.ReservedQuantity,
                        StockQuantity = item.StockQuantity,
                        WarehouseId = item.WarehouseId
                    };
                    list.Add(productInventory);
                }
            }

            if (list.Count > 0)
            {
                _productInventoryRepository.Add(list);
            }
            return await Commit(_productInventoryRepository.UnitOfWork);
        }
        public async Task<ValidationResult> Handle(ProductInventoryDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productInventoryRepository)) return request.ValidationResult;
            var productInventory = new ProductInventory
            {
                Id = request.Id
            };

            //add domain event
            //productInventory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productInventoryRepository.Remove(productInventory);
            return await Commit(_productInventoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductInventoryEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            ProductInventory dataProductInventory = await _productInventoryRepository.GetById(request.Id);
            var productInventory = new ProductInventory
            {
                Id = request.Id,
                ProductId = request.ProductId,
                PlannedQuantity = request.PlannedQuantity,
                ReservedQuantity = request.ReservedQuantity,
                StockQuantity = request.StockQuantity,
                WarehouseId = request.WarehouseId,
                CreatedBy = dataProductInventory.CreatedBy,
                CreatedDate = dataProductInventory.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //productInventory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productInventoryRepository.Update(productInventory);
            return await Commit(_productInventoryRepository.UnitOfWork);
        }
    }
}
