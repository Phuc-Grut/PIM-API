using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductReviewHelpfulnessValidation<T> : AbstractValidator<T> where T : ProductReviewHelpfulnessCommand

    {
        protected readonly IProductReviewHelpfulnessRepository _context;
        private Guid Id;
        public ProductReviewHelpfulnessValidation()
        {

        }
        public ProductReviewHelpfulnessValidation(IProductReviewHelpfulnessRepository context)
        {
            _context = context;
        }
        protected void ValidateIdExists()
        {
            RuleFor(x => x.Id).Must(IsExist).WithMessage("Id not exists");
        }
        private bool IsExist(Guid id)
        {
            return _context.CheckExistById(id).Result;
        }
        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Id");
        }
        protected void ValidateProductReviewId()
        {
            RuleFor(c => c.ProductReviewId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the ProductReviewId");
        }
    }
    public class ProductReviewHelpfulnessAddCommandValidation : ProductReviewHelpfulnessValidation<ProductReviewHelpfulnessAddCommand>
    {
        public ProductReviewHelpfulnessAddCommandValidation()
        {
            ValidateId();
            ValidateProductReviewId();
        }
    }
    public class ProductReviewHelpfulnessEditCommandValidation : ProductReviewHelpfulnessValidation<ProductReviewHelpfulnessEditCommand>
    {
        public ProductReviewHelpfulnessEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateProductReviewId();
        }
    }

    public class ProductReviewHelpfulnessDeleteCommandValidation : ProductReviewHelpfulnessValidation<ProductReviewHelpfulnessDeleteCommand>
    {
        public ProductReviewHelpfulnessDeleteCommandValidation(IProductReviewHelpfulnessRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
