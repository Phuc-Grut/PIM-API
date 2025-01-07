using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductServiceAddCommandHandler : CommandHandler, IRequestHandler<ProductServiceAddAddCommand, ValidationResult>, IRequestHandler<ProductServiceAddDeleteCommand, ValidationResult>, IRequestHandler<ProductServiceAddEditCommand, ValidationResult>
    {
        private readonly IProductServiceAddRepository _productServiceAddRepository;

        public ProductServiceAddCommandHandler(IProductServiceAddRepository ProductServiceAddRepository)
        {
            _productServiceAddRepository = ProductServiceAddRepository;
        }
        public void Dispose()
        {
            _productServiceAddRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductServiceAddAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productServiceAdd = new ProductServiceAdd
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ServiceAddId = request.ServiceAddId,
                PayRequired = request.PayRequired,
                Price = request.Price,
                MaxPrice = request.MaxPrice,
                CalculationMethod = request.CalculationMethod,
                PriceSyntax = request.PriceSyntax,
                MinPrice = request.MinPrice,
                Currency = request.Currency,
                Status = request.Status,
                CreatedBy = request.CreatedBy,
                CreatedDate= request.CreatedDate,
            };

            //add domain event
            //productServiceAdd.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productServiceAddRepository.Add(productServiceAdd);
            return await Commit(_productServiceAddRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductServiceAddDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productServiceAddRepository)) return request.ValidationResult;
            var productServiceAdd = new ProductServiceAdd
            {
                Id = request.Id
            };

            //add domain event
            //productServiceAdd.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productServiceAddRepository.Remove(productServiceAdd);
            return await Commit(_productServiceAddRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductServiceAddEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            ProductServiceAdd dataProductServiceAdd = await _productServiceAddRepository.GetById(request.Id);
            var productServiceAdd = new ProductServiceAdd
            {
                Id = request.Id,
                ProductId = request.ProductId,
                ServiceAddId = request.ServiceAddId,
                PayRequired = request.PayRequired,
                Price = request.Price,
                MaxPrice = request.MaxPrice,
                CalculationMethod = request.CalculationMethod,
                PriceSyntax = request.PriceSyntax,
                MinPrice = request.MinPrice,
                Currency = request.Currency,
                Status = request.Status,
                CreatedBy = dataProductServiceAdd.CreatedBy,
                CreatedDate = dataProductServiceAdd.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = dataProductServiceAdd.UpdatedByName,
                CreatedByName = dataProductServiceAdd.CreatedByName
            };

            //add domain event
            //productServiceAdd.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productServiceAddRepository.Update(productServiceAdd);
            return await Commit(_productServiceAddRepository.UnitOfWork);
        }
    }
}
