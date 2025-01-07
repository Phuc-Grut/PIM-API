using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;

namespace VFi.Application.PIM.Commands
{
    internal class ProductMediaCommandHandler : CommandHandler, IRequestHandler<ProductMediaAddCommand, ValidationResult>,
        IRequestHandler<ProductMediaAddListCommand, ValidationResult>,
        IRequestHandler<ProductMediaDeleteCommand, ValidationResult>, IRequestHandler<ProductMediaEditCommand, ValidationResult>
    {
        private readonly IProductMediaRepository _productMediaRepository;

        public ProductMediaCommandHandler(IProductMediaRepository productMediaRepository)
        {
            _productMediaRepository = productMediaRepository;
        }
        public void Dispose()
        {
            _productMediaRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductMediaAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productMedia = new ProductMedia
            {
                Id = request.Id,
                Name = request.Name,
                DisplayOrder = request.DisplayOrder,
                ProductId = request.ProductId,
                MediaType = request.MediaType,
                Path = request.Path,
                CreatedBy = request.CreatedBy,
                CreatedDate = request.CreatedDate
            };

            //add domain event
            //productMedia.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productMediaRepository.Add(productMedia);
            return await Commit(_productMediaRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductMediaDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productMediaRepository)) return request.ValidationResult;
            var productMedia = await _productMediaRepository.GetById(request.Id);
            if (productMedia != null)
            {
                _productMediaRepository.Remove(productMedia);
            }
            return await Commit(_productMediaRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductMediaEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            ProductMedia dataProductMedia = await _productMediaRepository.GetById(request.Id);
            var productMedia = new ProductMedia
            {
                Id = request.Id,
                Name = request.Name,
                DisplayOrder = request.DisplayOrder,
                ProductId = request.ProductId,
                MediaType = request.MediaType,
                Path = request.Path,
                CreatedBy = dataProductMedia.CreatedBy,
                CreatedDate = dataProductMedia.CreatedDate,
                UpdatedBy = request.UpdatedBy,
                UpdatedDate = request.UpdatedDate
            };

            //add domain event
            //productMedia.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productMediaRepository.Update(productMedia);
            return await Commit(_productMediaRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductMediaAddListCommand request, CancellationToken cancellationToken)
        {
            List<ProductMedia> list = new List<ProductMedia>();
            if (request.ListAtt?.Count > 0)
            {
                foreach (var item in request.ListAtt)
                {
                    var productMedia = new ProductMedia
                    {
                        Id = item.Id,
                        Name = item.Name,
                        DisplayOrder = item.DisplayOrder,
                        ProductId = item.ProductId,
                        MediaType = item.MediaType,
                        Path = item.Path,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    };
                    list.Add(productMedia);
                }
            }

            if (list.Count > 0)
            {
                _productMediaRepository.Add(list);
            }
            return await Commit(_productMediaRepository.UnitOfWork);
        }
    }
}
