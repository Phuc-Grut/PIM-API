using VFi.Domain.PIM.Interfaces;
using FluentValidation;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class CurrencyValidation<T> : AbstractValidator<T> where T : CurrencyCommand

    {
        protected readonly ICurrencyRepository _context;
        private Guid Id;
        public CurrencyValidation(ICurrencyRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        public CurrencyValidation(ICurrencyRepository context)
        {
            _context = context;
        }

        protected void ValidateAddCodeUnique()
        {
            RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code alrealy exists");
        }

        private bool IsAddUnique(string? code)
        {
            return !_context.CheckExist(code, null).Result;
        }
        protected void ValidateEditCodeUnique()
        {
            RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code alrealy exists");
        }
        private bool IsEditUnique(string? code)
        {
            return !_context.CheckExist(code, Id).Result;
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
        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .Length(0, 225).WithMessage("The name must have between 0 and 225 characters");
        }
        protected void ValidateLocale()
        {
            RuleFor(c => c.Locale)
                .Length(0, 50).WithMessage("The name must have between 0 and 50 characters");
        }
        protected void ValidateCustomFormatting()
        {
            RuleFor(c => c.CustomFormatting)
                .Length(0, 50).WithMessage("The name must have between 0 and 50 characters");
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status)
                .NotNull().WithMessage("Please ensure you have selected the status");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
    }
    public class CurrencyAddCommandValidation : CurrencyValidation<CurrencyAddCommand>
    {
        public CurrencyAddCommandValidation(ICurrencyRepository context) : base(context)
        {
            ValidateId();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateLocale();
            ValidateCustomFormatting();
            ValidateAddCodeUnique();
            ValidateName();
        }
    }
    public class CurrencyEditCommandValidation : CurrencyValidation<CurrencyEditCommand>
    {
        public CurrencyEditCommandValidation(ICurrencyRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateLocale();
            ValidateCustomFormatting();
            ValidateEditCodeUnique();
            ValidateCode();
            ValidateName();
        }
    }

    public class CurrencyDeleteCommandValidation : CurrencyValidation<CurrencyDeleteCommand>
    {
        public CurrencyDeleteCommandValidation(ICurrencyRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
