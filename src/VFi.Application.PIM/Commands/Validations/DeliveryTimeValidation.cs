using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class DeliveryTimeValidation<T> : AbstractValidator<T> where T : DeliveryTimeCommand
    {
        protected readonly IDeliveryTimeRepository _context;
        public DeliveryTimeValidation(IDeliveryTimeRepository context)
        {
            _context = context;
        }
        public DeliveryTimeValidation()
        {}
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
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the name")
                .Length(2, 400).WithMessage("The name must have between 2 and 225 characters");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class DeliveryTimeAddCommandValidation : DeliveryTimeValidation<DeliveryTimeAddCommand>
    {
        public DeliveryTimeAddCommandValidation() : base()
        {
            ValidateId();
            ValidateDisplayOrder();
            ValidateName();
        }
    }
    public class DeliveryTimeEditCommandValidation : DeliveryTimeValidation<DeliveryTimeEditCommand>
    {
        public DeliveryTimeEditCommandValidation(IDeliveryTimeRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateDisplayOrder();
            ValidateName();
        }
    }

    public class DeliveryTimeDeleteCommandValidation : DeliveryTimeValidation<DeliveryTimeDeleteCommand>
    {
        public DeliveryTimeDeleteCommandValidation(IDeliveryTimeRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
