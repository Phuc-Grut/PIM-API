using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class TaxCategoryValidation<T> : AbstractValidator<T> where T : TaxCategoryCommand

    {
        protected readonly ITaxCategoryRepository _context;
        private Guid Id;
        public TaxCategoryValidation(ITaxCategoryRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        public TaxCategoryValidation(ITaxCategoryRepository context)
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
                .NotEmpty().WithMessage("Please ensure you have entered the name")
                .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status)
                .NotNull().WithMessage("Please ensure you have selected the status");
        }
        protected void ValidateRate()
        {
            RuleFor(c => c.Rate)
                .NotNull().WithMessage("Please ensure you have selected the rate");
        }
        protected void ValidateGroup()
        {
            RuleFor(c => c.Group)
                .Length(0, 50).WithMessage("The group must have between 0 and 225 characters");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class TaxCategoryAddCommandValidation : TaxCategoryValidation<TaxCategoryAddCommand>
    {
        public TaxCategoryAddCommandValidation(ITaxCategoryRepository context) : base(context)
        {
            ValidateId();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
            ValidateRate();
            ValidateGroup();
            ValidateAddCodeUnique();
        }
    }
    public class TaxCategoryEditCommandValidation : TaxCategoryValidation<TaxCategoryEditCommand>
    {
        public TaxCategoryEditCommandValidation(ITaxCategoryRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStatus();
            ValidateEditCodeUnique();
            ValidateDisplayOrder();
            ValidateRate();
            ValidateGroup();
            ValidateCode();
            ValidateName();
        }
    }

    public class TaxCategoryDeleteCommandValidation : TaxCategoryValidation<TaxCategoryDeleteCommand>
    {
        public TaxCategoryDeleteCommandValidation(ITaxCategoryRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
