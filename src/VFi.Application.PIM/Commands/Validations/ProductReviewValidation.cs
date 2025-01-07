using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductReviewValidation<T> : AbstractValidator<T> where T : ProductReviewCommand

    {
        protected readonly IProductReviewRepository _context;
        private Guid Id;
        public ProductReviewValidation()
        {
        }
        public ProductReviewValidation(IProductReviewRepository context)
        {
            _context = context;
        }
        protected void ValidateIdExists() {
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
        protected void ValidateProductId()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the ProductId");
        }
        protected void ValidateTitle()
        {
            RuleFor(c => c.Title)
                .Length(0, 50).WithMessage("The title must have between 0 and 40000 characters");
        }
     
    }
    public class ProductReviewAddCommandValidation : ProductReviewValidation<ProductReviewAddCommand>
    {
        public ProductReviewAddCommandValidation()
        {
            ValidateId();
            ValidateTitle();
            ValidateProductId();
        }
    }
    public class ProductReviewEditCommandValidation : ProductReviewValidation<ProductReviewEditCommand>
    {
        public ProductReviewEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateTitle();
            ValidateProductId();
        }
    }

    public class ProductReviewDeleteCommandValidation : ProductReviewValidation<ProductReviewDeleteCommand>
    {
        public ProductReviewDeleteCommandValidation(IProductReviewRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
