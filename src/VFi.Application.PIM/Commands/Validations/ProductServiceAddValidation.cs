using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductServiceAddValidation<T> : AbstractValidator<T> where T : ProductServiceAddCommand

    {
        protected readonly IProductServiceAddRepository _context;
        private Guid Id;
        public ProductServiceAddValidation()
        {
        }
        public ProductServiceAddValidation(IProductServiceAddRepository context)
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
    }
    public class ProductServiceAddAddCommandValidation : ProductServiceAddValidation<ProductServiceAddAddCommand>
    {
        public ProductServiceAddAddCommandValidation()
        {
            ValidateId();
            ValidateProductId();
        }
    }
    public class ProductServiceAddEditCommandValidation : ProductServiceAddValidation<ProductServiceAddEditCommand>
    {
        public ProductServiceAddEditCommandValidation(IProductServiceAddRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateProductId();
        }
    }

    public class ProductServiceAddDeleteCommandValidation : ProductServiceAddValidation<ProductServiceAddDeleteCommand>
    {
        public ProductServiceAddDeleteCommandValidation(IProductServiceAddRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
