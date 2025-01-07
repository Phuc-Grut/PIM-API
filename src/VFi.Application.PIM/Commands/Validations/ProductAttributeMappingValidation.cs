using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductAttributeMappingValidation<T> : AbstractValidator<T> where T : ProductAttributeMappingCommand

    {
        protected readonly IProductProductAttributeMappingRepository _context;
        public ProductAttributeMappingValidation(IProductProductAttributeMappingRepository context)
        {
            _context = context;
        }
        public ProductAttributeMappingValidation()
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
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Product");
        }  protected void ValidateProductAttributeId()
        {
            RuleFor(c => c.ProductAttributeId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Product Attribute");
        }
       
    }
    public class ProductAttributeMappingAddCommandValidation : ProductAttributeMappingValidation<ProductAttributeMappingAddCommand>
    {
        public ProductAttributeMappingAddCommandValidation()
        {
            ValidateId();
            ValidateProductId();
            ValidateProductAttributeId();
        }
    }
    public class ProductAttributeMappingEditCommandValidation : ProductAttributeMappingValidation<ProductAttributeMappingEditCommand>
    {
        public ProductAttributeMappingEditCommandValidation(IProductProductAttributeMappingRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateProductId();
            ValidateProductAttributeId();
        }
    }

    public class ProductAttributeMappingDeleteCommandValidation : ProductAttributeMappingValidation<ProductAttributeMappingDeleteCommand>
    {
        public ProductAttributeMappingDeleteCommandValidation(IProductProductAttributeMappingRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
