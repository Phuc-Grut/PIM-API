using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductCategoryMappingValidation<T> : AbstractValidator<T> where T : ProductCategoryMappingCommand

    {
        protected readonly IProductCategoryMappingRepository _context;
    
        public ProductCategoryMappingValidation(IProductCategoryMappingRepository context)
        {
            _context = context;
        }
        public ProductCategoryMappingValidation()
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
        protected void ValidateCategoryId()
        {
            RuleFor(c => c.CategoryId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the CategoryId");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class ProductCategoryMappingAddCommandValidation : ProductCategoryMappingValidation<ProductCategoryMappingAddCommand>
    {
        public ProductCategoryMappingAddCommandValidation() 
        {
            ValidateId();
            ValidateCategoryId();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }
    public class ProductCategoryMappingEditCommandValidation : ProductCategoryMappingValidation<ProductCategoryMappingEditCommand>
    {
        public ProductCategoryMappingEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateCategoryId();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }

    public class ProductCategoryMappingDeleteCommandValidation : ProductCategoryMappingValidation<ProductCategoryMappingDeleteCommand>
    {
        public ProductCategoryMappingDeleteCommandValidation(IProductCategoryMappingRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
