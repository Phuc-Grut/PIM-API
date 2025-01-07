using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation.Results;
using MediatR;

namespace VFi.Application.PIM.Commands
{
    internal class ProductReviewHelpfulnessCommandHandler : CommandHandler, IRequestHandler<ProductReviewHelpfulnessAddCommand, ValidationResult>, IRequestHandler<ProductReviewHelpfulnessDeleteCommand, ValidationResult>, IRequestHandler<ProductReviewHelpfulnessEditCommand, ValidationResult>
    {
        private readonly IProductReviewHelpfulnessRepository _productReviewHelpfulnessRepository;

        public ProductReviewHelpfulnessCommandHandler(IProductReviewHelpfulnessRepository ProductReviewHelpfulnessRepository)
        {
            _productReviewHelpfulnessRepository = ProductReviewHelpfulnessRepository;
        }
        public void Dispose()
        {
            _productReviewHelpfulnessRepository.Dispose();
        }

        public async Task<ValidationResult> Handle(ProductReviewHelpfulnessAddCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productReviewHelpfulness = new ProductReviewHelpfulness
            {
                Id = request.Id,
                ProductReviewId = request.ProductReviewId,
                WasHelpful = request.WasHelpful
            };

            //add domain event
            //productReviewHelpfulness.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewHelpfulnessRepository.Add(productReviewHelpfulness);
            return await Commit(_productReviewHelpfulnessRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductReviewHelpfulnessDeleteCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid(_productReviewHelpfulnessRepository)) return request.ValidationResult;
            var productReviewHelpfulness = new ProductReviewHelpfulness
            {
                Id = request.Id
            };

            //add domain event
            //productReviewHelpfulness.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewHelpfulnessRepository.Remove(productReviewHelpfulness);
            return await Commit(_productReviewHelpfulnessRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(ProductReviewHelpfulnessEditCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;
            var productReviewHelpfulness = new ProductReviewHelpfulness
            {
                Id = request.Id,
                ProductReviewId = request.ProductReviewId,
                WasHelpful = request.WasHelpful
            };

            //add domain event
            //productReviewHelpfulness.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

            _productReviewHelpfulnessRepository.Update(productReviewHelpfulness);
            return await Commit(_productReviewHelpfulnessRepository.UnitOfWork);
        }
    }
}
