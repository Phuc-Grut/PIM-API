using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class RelatedProductCommandHandler : CommandHandler, IRequestHandler<RelatedProductAddCommand, ValidationResult>, IRequestHandler<RelatedProductDeleteCommand, ValidationResult>, IRequestHandler<RelatedProductEditCommand, ValidationResult>
    {
        private readonly IRelatedProductRepository _relatedProduct;

        public RelatedProductCommandHandler(IRelatedProductRepository RelatedProductRepository)
        {
            _relatedProduct = RelatedProductRepository;
        }
        public void Dispose()
        {
            _relatedProduct.Dispose();
        }

        public async Task<ValidationResult> Handle(RelatedProductAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            List<RelatedProduct> list = new List<RelatedProduct>();
            if (request.ListProductId2?.Count > 0)
            {
                var i = 0;
                foreach (var item in request.ListProductId2)
                {
                    var relatedProduct = new RelatedProduct
                    {
                        Id = Guid.NewGuid(),
                        ProductId1 = request.ProductId1,
                        ProductId2 = item,
                        DisplayOrder = i
                    };
                    i++;
                    list.Add(relatedProduct);
                }
            }

            if (list.Count > 0)
            {
                _relatedProduct.Add(list);
            }
            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            return await Commit(_relatedProduct.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(RelatedProductDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_relatedProduct)) return request.ValidationResult;
            var tierPrice = new RelatedProduct
            {
                Id = request.Id
            };

            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _relatedProduct.Remove(tierPrice);
            return await Commit(_relatedProduct.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(RelatedProductEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            RelatedProduct dataRelatedProduct = await _relatedProduct.GetById(request.Id);
            var tierPrice = new RelatedProduct
            {
                Id = request.Id,
                ProductId1 = request.ProductId1,
                ProductId2 = request.ProductId2,
                DisplayOrder = request.DisplayOrder
            };

            //add domain event
            //tierPrice.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _relatedProduct.Update(tierPrice);
            return await Commit(_relatedProduct.UnitOfWork);
        }
    }
}
