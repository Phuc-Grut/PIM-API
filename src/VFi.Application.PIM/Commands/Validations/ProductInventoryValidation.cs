using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductInventoryValidation<T> : AbstractValidator<T> where T : ProductInventoryCommand

    {
        protected readonly IProductInventoryRepository _context;
        private Guid Id;
        public ProductInventoryValidation()
        {
        }
        public ProductInventoryValidation(IProductInventoryRepository context)
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
        protected void ValidateWarehouseId()
        {
            RuleFor(c => c.WarehouseId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the WarehouseId");
        }
        protected void ValidateProductId()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the ProductId");
        }
    }
    public class ProductInventoryAddCommandValidation : ProductInventoryValidation<ProductInventoryAddCommand>
    {
        public ProductInventoryAddCommandValidation()
        {
            ValidateId();
            ValidateWarehouseId();
            ValidateProductId();
        }
    }
    public class ProductInventoryEditCommandValidation : ProductInventoryValidation<ProductInventoryEditCommand>
    {
        public ProductInventoryEditCommandValidation(IProductInventoryRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateWarehouseId();
            ValidateProductId();
        }
    }

    public class ProductInventoryDeleteCommandValidation : ProductInventoryValidation<ProductInventoryDeleteCommand>
    {
        public ProductInventoryDeleteCommandValidation(IProductInventoryRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
