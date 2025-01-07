using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductSpecificationAttributeMappingValidation<T> : AbstractValidator<T> where T : ProductSpecificationAttributeMappingCommand

    {
        protected readonly IProductSpecificationAttributeMappingRepository _context;
    
        public ProductSpecificationAttributeMappingValidation(IProductSpecificationAttributeMappingRepository context)
        {
            _context = context;
        }
        public ProductSpecificationAttributeMappingValidation()
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
        protected void ValidateSpecificationAttributeOptionId()
        {
            RuleFor(c => c.SpecificationAttributeOptionId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the SpecificationAttributeOptionId");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class ProductSpecificationAttributeMappingAddCommandValidation : ProductSpecificationAttributeMappingValidation<ProductSpecificationAttributeMappingAddCommand>
    {
        public ProductSpecificationAttributeMappingAddCommandValidation() 
        {
            ValidateId();
            ValidateSpecificationAttributeOptionId();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }
    public class ProductSpecificationAttributeMappingEditCommandValidation : ProductSpecificationAttributeMappingValidation<ProductSpecificationAttributeMappingEditCommand>
    {
        public ProductSpecificationAttributeMappingEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateSpecificationAttributeOptionId();
            ValidateDisplayOrder();
            ValidateProductId();
        }
    }

    public class ProductSpecificationAttributeMappingDeleteCommandValidation : ProductSpecificationAttributeMappingValidation<ProductSpecificationAttributeMappingDeleteCommand>
    {
        public ProductSpecificationAttributeMappingDeleteCommandValidation(IProductSpecificationAttributeMappingRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
