using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductMediaValidation<T> : AbstractValidator<T> where T : ProductMediaCommand

    {
        protected readonly IProductMediaRepository _context;
        private Guid Id;
        public ProductMediaValidation()
        {
        }
        public ProductMediaValidation(IProductMediaRepository context)
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
        protected void ValidatePath()
        {
            RuleFor(c => c.Path)
                .NotEmpty().WithMessage("Please ensure you have entered the path")
                .Length(0, 225).WithMessage("The code must have between 0 and 225 characters");
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
        }
        protected void ValidateMediaType()
        {
            RuleFor(c => c.MediaType)
                .NotNull().WithMessage("Please ensure you have selected the media type");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class ProductMediaAddCommandValidation : ProductMediaValidation<ProductMediaAddCommand>
    {
        public ProductMediaAddCommandValidation()
        {
            ValidateId();
            ValidatePath();
            ValidateDisplayOrder();
            ValidateName();
            ValidateMediaType();
            ValidateProductId();
        }
    }
    public class ProductMediaEditCommandValidation : ProductMediaValidation<ProductMediaEditCommand>
    {
        public ProductMediaEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidatePath();
            ValidateDisplayOrder();
            ValidateName();
            ValidateMediaType();
            ValidateProductId();
        }
    }

    public class ProductMediaDeleteCommandValidation : ProductMediaValidation<ProductMediaDeleteCommand>
    {
        public ProductMediaDeleteCommandValidation(IProductMediaRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
