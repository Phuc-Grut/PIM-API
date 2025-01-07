using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands.Handler
{
    internal class ProductTagCommandHandler : CommandHandler, IRequestHandler<ProductTagAddCommand, ValidationResult>, IRequestHandler<ProductTagDeleteCommand, ValidationResult>, IRequestHandler<ProductTagEditCommand, ValidationResult>
    {
        private readonly IProductTagRepository _productTagRepository;

        public ProductTagCommandHandler(IProductTagRepository ProductTagRepository)
        {
            _productTagRepository = ProductTagRepository;
        }
        public void Dispose()
        {
            _productTagRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductTagAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productTag = new ProductTag
            {
                Id = request.Id,
                Status = request.Status,
                Name = request.Name,
                Type = request.Type,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate
            };

            //add domain event
            //productTag.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productTagRepository.Add(productTag);
            return await Commit(_productTagRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductTagDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productTagRepository)) return request.ValidationResult;
            var productTag = new ProductTag
            {
                Id = request.Id
            };

            //add domain event
            //productTag.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            try
            {
                _productTagRepository.Remove(productTag);
                return await Commit(_productTagRepository.UnitOfWork);
            }
            catch (Exception ex)
            {
                return new ValidationResult(new List<ValidationFailure>() { new ValidationFailure("id", "In use, cannot be deleted") });
            }
        }

        public async Task<ValidationResult> Handle(ProductTagEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productTagRepository)) return request.ValidationResult;
            var data = await _productTagRepository.GetById(request.Id);
            var productTag = new ProductTag
            {
                Id = request.Id,
                Status = request.Status,
                Name = request.Name,
                Type = request.Type,
                CreatedBy = data.CreatedBy,
                CreatedDate = data.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //productTag.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productTagRepository.Update(productTag);
            return await Commit(_productTagRepository.UnitOfWork);
        }
    }
}
