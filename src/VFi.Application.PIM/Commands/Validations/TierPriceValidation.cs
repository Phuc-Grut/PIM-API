using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class TierPriceValidation<T> : AbstractValidator<T> where T : TierPriceCommand

    {
        protected readonly ITierPriceRepository _context;
        public TierPriceValidation(ITierPriceRepository context)
        {
            _context = context;
        }
        public TierPriceValidation()
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
        protected void ValidateStoreId()
        {
            RuleFor(c => c.StoreId)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the StoreId");
        }
    }
    public class TierPriceAddCommandValidation : TierPriceValidation<TierPriceAddCommand>
    {
        public TierPriceAddCommandValidation()
        {
            ValidateId();
            ValidateStoreId();
            ValidateProductId();
        }
    }
    public class TierPriceEditCommandValidation : TierPriceValidation<TierPriceEditCommand>
    {
        public TierPriceEditCommandValidation(ITierPriceRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStoreId();
            ValidateProductId();
        }
    }

    public class TierPriceDeleteCommandValidation : TierPriceValidation<TierPriceDeleteCommand>
    {
        public TierPriceDeleteCommandValidation(ITierPriceRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
