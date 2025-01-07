using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductSpecificationCodeValidation<T> : AbstractValidator<T> where T : ProductSpecificationCodeCommand

    {
        protected readonly IProductSpecificationCodeRepository _context;
    
        public ProductSpecificationCodeValidation(IProductSpecificationCodeRepository context)
        {
            _context = context;
        }
        public ProductSpecificationCodeValidation()
        {
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
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class ProductSpecificationCodeAddCommandValidation : ProductSpecificationCodeValidation<ProductSpecificationCodeAddCommand>
    {
        public ProductSpecificationCodeAddCommandValidation() 
        {
            ValidateId();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }
    public class ProductSpecificationCodeEditCommandValidation : ProductSpecificationCodeValidation<ProductSpecificationCodeEditCommand>
    {
        public ProductSpecificationCodeEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }

    public class ProductSpecificationCodeDeleteCommandValidation : ProductSpecificationCodeValidation<ProductSpecificationCodeDeleteCommand>
    {
        public ProductSpecificationCodeDeleteCommandValidation(IProductSpecificationCodeRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
