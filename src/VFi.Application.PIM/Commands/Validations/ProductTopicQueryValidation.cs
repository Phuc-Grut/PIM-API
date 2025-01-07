using VFi.Domain.PIM.Interfaces;
using FluentValidation;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductTopicQueryValidation<T> : AbstractValidator<T> where T : ProductTopicQueryCommand

    {
        protected readonly IProductTopicQueryRepository _context;
        private Guid Id;
        public ProductTopicQueryValidation(IProductTopicQueryRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        public ProductTopicQueryValidation(IProductTopicQueryRepository context)
        {
            _context = context;
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
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
    }
    public class ProductTopicQueryAddCommandValidation : ProductTopicQueryValidation<AddProductTopicQueryCommand>
    {
        public ProductTopicQueryAddCommandValidation(IProductTopicQueryRepository context) : base(context)
        {
            ValidateId();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateName();
        }
    }
    public class ProductTopicQueryEditCommandValidation : ProductTopicQueryValidation<EditProductTopicQueryCommand>
    {
        public ProductTopicQueryEditCommandValidation(IProductTopicQueryRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateName();
        }
    }

    public class ProductTopicQueryDeleteCommandValidation : ProductTopicQueryValidation<DeleteProductTopicQueryCommand>
    {
        public ProductTopicQueryDeleteCommandValidation(IProductTopicQueryRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
