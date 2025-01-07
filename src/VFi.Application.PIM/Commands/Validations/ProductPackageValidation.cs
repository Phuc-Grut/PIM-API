using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductPackageValidation<T> : AbstractValidator<T> where T : ProductPackageCommand

    {
        protected readonly IProductPackageRepository _context;
        private Guid Id;
        public ProductPackageValidation()
        {
        }
        public ProductPackageValidation(IProductPackageRepository context)
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
    public class ProductPackageAddCommandValidation : ProductPackageValidation<ProductPackageAddCommand>
    {
        public ProductPackageAddCommandValidation()
        {
            ValidateId();
            ValidateProductId();
        }
    }
    public class ProductPackageEditCommandValidation : ProductPackageValidation<ProductPackageEditCommand>
    {
        public ProductPackageEditCommandValidation(IProductPackageRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateProductId();
        }
    }

    public class ProductPackageDeleteCommandValidation : ProductPackageValidation<ProductPackageDeleteCommand>
    {
        public ProductPackageDeleteCommandValidation(IProductPackageRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
