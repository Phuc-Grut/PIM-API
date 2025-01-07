using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductVariantAttributeCombinationValidation<T> : AbstractValidator<T> where T : ProductVariantAttributeCombinationCommand

    {
        protected readonly IProductVariantAttributeCombinationRepository _context;

        public ProductVariantAttributeCombinationValidation(IProductVariantAttributeCombinationRepository context)
        {
            _context = context;
        }
        public ProductVariantAttributeCombinationValidation()
        {
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
        protected void ValidateProductId()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the ProductId");
        }
 
        protected void ValidateAllowOutOfStockOrders()
        {
            RuleFor(c => c.AllowOutOfStockOrders)
                      .NotNull().WithMessage("Please ensure you have entered the AllowOutOfStockOrders");
        }
        protected void ValidateStockQuantity()
        {
            RuleFor(c => c.StockQuantity)
                      .NotNull().WithMessage("Please ensure you have entered the StockQuantity");
        }
      
     
    }
    public class ProductVariantAttributeCombinationAddCommandValidation : ProductVariantAttributeCombinationValidation<ProductVariantAttributeCombinationAddCommand>
    {
        public ProductVariantAttributeCombinationAddCommandValidation()
        {
            ValidateId();
            ValidateProductId();
            ValidateStockQuantity();
            ValidateAllowOutOfStockOrders();
          
        }
    }
    public class ProductVariantAttributeCombinationEditCommandValidation : ProductVariantAttributeCombinationValidation<ProductVariantAttributeCombinationEditCommand>
    {
        public ProductVariantAttributeCombinationEditCommandValidation()
        {
            ValidateId();
            ValidateIdExists();
            ValidateStockQuantity();
            ValidateAllowOutOfStockOrders();
            ValidateProductId();
        }
    }

    public class ProductVariantAttributeCombinationDeleteCommandValidation : ProductVariantAttributeCombinationValidation<ProductVariantAttributeCombinationDeleteCommand>
    {
        public ProductVariantAttributeCombinationDeleteCommandValidation(IProductVariantAttributeCombinationRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
