using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MassTransit.Mediator;
using MediatR;

namespace VFi.Application.PIM.Commands.Handler
{
    internal class ProductOriginCommandHandler : CommandHandler, IRequestHandler<ProductOriginAddCommand, ValidationResult>,
                                                                IRequestHandler<ProductOriginDeleteCommand, ValidationResult>,
                                                                IRequestHandler<ProductOriginEditCommand, ValidationResult>,
                                                                IRequestHandler<ProductOriginSortCommand, ValidationResult>
    {
        private readonly IProductOriginRepository _productOriginRepository;
        private readonly IProductRepository _productRepository;

        public ProductOriginCommandHandler(IProductOriginRepository productOriginRepository, IProductRepository productRepository)
        {
            _productOriginRepository = productOriginRepository;
            _productRepository = productRepository;
        }
        public void Dispose()
        {
            _productOriginRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductOriginAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productOriginRepository)) return request.ValidationResult;
            var productOrigin = new ProductOrigin
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate,
                CreatedByName = request.CreatedByName
            };

            //add domain event
            //productOrigin.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productOriginRepository.Add(productOrigin);
            return await Commit(_productOriginRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductOriginDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productOriginRepository)) return request.ValidationResult;

            var filter = new Dictionary<string, object> { { "originId", request.Id } };

            var products = await _productRepository.Filter(filter);
            if (products.Any())
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }

            var productOrigin = new ProductOrigin
            {
                Id = request.Id
            };

            _productOriginRepository.Remove(productOrigin);
            return await Commit(_productOriginRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductOriginEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productOriginRepository)) return request.ValidationResult;
            var data = await _productOriginRepository.GetById(request.Id);

            var productOrigin = new ProductOrigin
            {
                Id = request.Id,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                DisplayOrder = request.DisplayOrder,
                CreatedBy = data.CreatedBy,
                CreatedByName = data.CreatedByName,
                CreatedDate = data.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate,
                UpdatedByName = request.UpdatedByName
            };

            //add domain event
            //productOrigin.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productOriginRepository.Update(productOrigin);
            return await Commit(_productOriginRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductOriginSortCommand request, CancellationToken cancellationToken)
        {
            var data = await _productOriginRepository.GetAll();

            List<ProductOrigin> productOrigins = new List<ProductOrigin>();

            foreach (var sort in request.SortList)
            {
                ProductOrigin productOrigin = data.FirstOrDefault(c => c.Id == sort.Id);
                if (productOrigin != null)
                {
                    productOrigin.DisplayOrder = sort.SortOrder;
                    productOrigins.Add(productOrigin);
                }
            }
            _productOriginRepository.Update(productOrigins);
            return await Commit(_productOriginRepository.UnitOfWork);
        }
    }
}
