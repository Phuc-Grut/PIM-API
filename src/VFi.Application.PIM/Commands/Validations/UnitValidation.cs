using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public class UnitValidation<T> : AbstractValidator<T> where T : UnitCommand

    {
        protected readonly IUnitRepository _context;
        private Guid Id;
        public UnitValidation(IUnitRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        public UnitValidation(IUnitRepository context)
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
            RuleFor(x => x.Id).Must(IsExist).WithMessage("Id nots exists");
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
                .NotEmpty().WithMessage("Please ensure you have entered the name")
                .Length(1, 400).WithMessage("The name must have between 2 and 400 characters");
        }
        protected void ValidateNamePlural()
        {
            RuleFor(c => c.NamePlural)
                .Length(0, 50).WithMessage("The name plural must have between 0 and 50 characters");
        }
        protected void ValidateDisplayLocale()
        {
            RuleFor(c => c.DisplayLocale)
                .Length(0, 50).WithMessage("The display locale must have between 0 and 50 characters");
        }
        protected void ValidateIsDefault()
        {
            RuleFor(c => c.IsDefault)
                .NotNull().WithMessage("Please ensure you have input the isDefault");
        }
        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                .Length(0, 50).WithMessage("The description must have between 0 and 500 characters");
        }
        protected void ValidateRate()
        {
            RuleFor(c => c.Rate)
                .NotNull().WithMessage("Please ensure you have selected the rate");
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
    public class UnitAddCommandValidation : UnitValidation<UnitAddCommand>
    {
        public UnitAddCommandValidation(IUnitRepository context) : base(context)
        {
            ValidateId();
            ValidateStatus();
           // ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
            //ValidateNamePlural();
            //ValidateIsDefault();
            //ValidateDescription();
            ValidateRate();
            //ValidateDisplayLocale();
            ValidateAddCodeUnique();
        }
    }
    public class UnitEditCommandValidation : UnitValidation<UnitEditCommand>
    {
        public UnitEditCommandValidation(IUnitRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            //ValidateIdExists();
            ValidateStatus();
            ValidateEditCodeUnique();
            //ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
            //ValidateNamePlural();
            //ValidateIsDefault();
            //ValidateDescription();
            ValidateRate();
            //ValidateDisplayLocale();
        }
    }

    public class UnitDeleteCommandValidation : UnitValidation<UnitDeleteCommand>
    {
        public UnitDeleteCommandValidation(IUnitRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
