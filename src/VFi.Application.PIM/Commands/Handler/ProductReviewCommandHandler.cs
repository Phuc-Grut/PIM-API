using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductReviewCommandHandler : CommandHandler, IRequestHandler<ProductReviewAddCommand, ValidationResult>, IRequestHandler<ProductReviewDeleteCommand, ValidationResult>, IRequestHandler<ProductReviewEditCommand, ValidationResult>
    {
        private readonly IProductReviewRepository _productReviewRepository;

        public ProductReviewCommandHandler(IProductReviewRepository ProductReviewRepository)
        {
            _productReviewRepository = ProductReviewRepository;
        }
        public void Dispose()
        {
            _productReviewRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductReviewAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productReview = new ProductReview
            {
                Id = request.Id,
                ProductId = request.ProductId,
                HelpfulNoTotal = request.HelpfulNoTotal,
                HelpfulYesTotal = request.HelpfulYesTotal,
                IsVerifiedPurchase = request.IsVerifiedPurchase,
                Rating = request.Rating,
                ReviewText = request.ReviewText,
                Title = request.Title,
            };

            //add domain event
            //productReview.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewRepository.Add(productReview);
            return await Commit(_productReviewRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductReviewDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productReviewRepository)) return request.ValidationResult;
            var productReview = new ProductReview
            {
                Id = request.Id
            };

            //add domain event
            //productReview.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewRepository.Remove(productReview);
            return await Commit(_productReviewRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductReviewEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productReview = new ProductReview
            {
                Id = request.Id,
                ProductId = request.ProductId,
                HelpfulNoTotal = request.HelpfulNoTotal,
                HelpfulYesTotal = request.HelpfulYesTotal,
                IsVerifiedPurchase = request.IsVerifiedPurchase,
                Rating = request.Rating,
                ReviewText = request.ReviewText,
                Title = request.Title,
            };

            //add domain event
            //productReview.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewRepository.Update(productReview);
            return await Commit(_productReviewRepository.UnitOfWork);
        }
    }
}
