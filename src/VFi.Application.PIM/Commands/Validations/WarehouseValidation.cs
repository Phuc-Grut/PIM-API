using VFi.Domain.PIM.Interfaces;
using FluentValidation;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class WarehouseValidation<T> : AbstractValidator<T> where T : WarehouseCommand

    {
        protected readonly IWarehouseRepository _context;
        private Guid Id;

        public WarehouseValidation(IWarehouseRepository context)
        {
            _context = context;
        }
        public WarehouseValidation(IWarehouseRepository context, Guid id)
        {
            _context = context;
            Id = id;
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
                .NotEmpty().WithMessage("Please ensure you have entered the code")
                .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the name")
                .Length(2, 50).WithMessage("The name must have between 2 and 50 characters");
        }
        protected void ValidateCompany()
        {
            RuleFor(c => c.Company)
                .Length(0, 225).WithMessage("The company must have between 2 and 50 characters");
        }
        protected void ValidateCountry()
        {
            RuleFor(c => c.Country)
                .Length(0, 225).WithMessage("The country must have between 0 and 225 characters");
        }
        protected void ValidateProvince()
        {
            RuleFor(c => c.Province)
                .Length(0, 225).WithMessage("The province must have between 0 and 225 characters");
        }
        protected void ValidateDistrict()
        {
            RuleFor(c => c.District)
                .Length(0, 225).WithMessage("The district must have between 0 and 225 characters");
        }
        protected void ValidateWard()
        {
            RuleFor(c => c.Ward)
                .Length(0, 225).WithMessage("The ward must have between 0 and 225 characters");
        }
        protected void ValidateAddress()
        {
            RuleFor(c => c.Address)
                .Length(0, 225).WithMessage("The address must have between 0 and 225 characters");
        }
        protected void ValidatePostalCode()
        {
            RuleFor(c => c.PostalCode)
                .Length(0, 50).WithMessage("The postal code must have between 2 and 50 characters");
        }
        protected void ValidatPhoneNumber()
        {
            RuleFor(c => c.PhoneNumber)
                .Length(0, 50).WithMessage("The phone number must have between 2 and 50 characters");
        }
        protected void ValidateApi()
        {
            RuleFor(c => c.Api)
                .Length(0, 225).WithMessage("The api must have between 0 and 225 characters");
        }
        protected void ValidateToken()
        {
            RuleFor(c => c.Token)
                .Length(0, 500).WithMessage("The token must have between 0 and 500 characters");
        }
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
    }
    public class WarehouseAddCommandValidation : WarehouseValidation<WarehouseAddCommand>
    {
        public WarehouseAddCommandValidation(IWarehouseRepository context) : base(context)
        {
            ValidateId();
            ValidateCompany();
            ValidateCountry();
            ValidateProvince();
            ValidateDistrict();
            ValidateWard();
            ValidateAddress();
            ValidatePostalCode();
            ValidatPhoneNumber();
            ValidateApi();
            ValidateAddCodeUnique();
            ValidateToken();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
        }
    }
    public class WarehouseEditCommandValidation : WarehouseValidation<WarehouseEditCommand>
    {
        public WarehouseEditCommandValidation(IWarehouseRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateCompany();
            ValidateCountry();
            ValidateProvince();
            ValidateDistrict();
            ValidateWard();
            ValidateAddress();
            ValidatePostalCode();
            ValidatPhoneNumber();
            ValidateApi();
            ValidateEditCodeUnique();
            ValidateToken();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
        }
    }

    public class WarehouseDeleteCommandValidation : WarehouseValidation<WarehouseDeleteCommand>
    {
        public WarehouseDeleteCommandValidation(IWarehouseRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
