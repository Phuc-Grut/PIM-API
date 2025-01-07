using VFi.Domain.PIM.Interfaces;
using FluentValidation;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductTopicDetailValidation<T> : AbstractValidator<T> where T : ProductTopicDetailCommand

    {
        protected readonly IProductTopicDetailRepository _context;
        private Guid Id;

        public ProductTopicDetailValidation(IProductTopicDetailRepository context)
        {
            _context = context;
        }
        public ProductTopicDetailValidation(IProductTopicDetailRepository context, Guid id)
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
         
    }
    public class ProductTopicDetailAddCommandValidation : ProductTopicDetailValidation<ProductTopicDetailAddCommand>
    {
        public ProductTopicDetailAddCommandValidation(IProductTopicDetailRepository context) : base(context)
        {
            ValidateId(); 
            ValidateAddCodeUnique(); 
            ValidateCode();
            ValidateName();
        }
    }
    public class ProductTopicDetailEditCommandValidation : ProductTopicDetailValidation<ProductTopicDetailEditCommand>
    {
        public ProductTopicDetailEditCommandValidation(IProductTopicDetailRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists(); 
            ValidateEditCodeUnique(); 
            ValidateCode();
            ValidateName();
        }
    }

    public class ProductTopicDetailDeleteCommandValidation : ProductTopicDetailValidation<ProductTopicDetailDeleteCommand>
    {
        public ProductTopicDetailDeleteCommandValidation(IProductTopicDetailRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
