using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductPackageCommandHandler : CommandHandler, IRequestHandler<ProductPackageAddCommand, ValidationResult>, IRequestHandler<ProductPackageDeleteCommand, ValidationResult>, IRequestHandler<ProductPackageEditCommand, ValidationResult>
    {
        private readonly IProductPackageRepository _productInventoryRepository;

        public ProductPackageCommandHandler(IProductPackageRepository ProductPackageRepository)
        {
            _productInventoryRepository = ProductPackageRepository;
        }
        public void Dispose()
        {
            _productInventoryRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductPackageAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productInventory = new ProductPackage
            {
                Id = request.Id,
                ProductId= request.ProductId,
                Name = request.Name,
                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
            };

            //add domain event
            //productInventory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productInventoryRepository.Add(productInventory);
            return await Commit(_productInventoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductPackageDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productInventoryRepository)) return request.ValidationResult;
            var productInventory = new ProductPackage
            {
                Id = request.Id
            };

            //add domain event
            //productInventory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productInventoryRepository.Remove(productInventory);
            return await Commit(_productInventoryRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductPackageEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            ProductPackage dataProductPackage = await _productInventoryRepository.GetById(request.Id);
            var productInventory = new ProductPackage
            {
                Id = request.Id,
                ProductId = request.ProductId,
                Name = request.Name,
                Weight = request.Weight,
                Length = request.Length,
                Width = request.Width,
                Height = request.Height,
                CreatedBy = dataProductPackage.CreatedBy,
                CreatedDate = dataProductPackage.CreatedDate,
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
