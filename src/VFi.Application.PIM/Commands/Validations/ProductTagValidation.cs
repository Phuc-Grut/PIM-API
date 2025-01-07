using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductTagValidation<T> : AbstractValidator<T> where T : ProductTagCommand

    {
        protected readonly IProductTagRepository _context;
        public ProductTagValidation(IProductTagRepository context)
        {
            _context = context;
        }
        public ProductTagValidation()
        { }
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
                .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
        }
        protected void ValidateStatus()
        {
            RuleFor(c => c.Status)
                .NotNull().WithMessage("Please ensure you have selected the status");
        }
    }
    public class ProductTagAddCommandValidation : ProductTagValidation<ProductTagAddCommand>
    {
        public ProductTagAddCommandValidation() : base()
        {
            ValidateId();
            ValidateStatus();
            ValidateName();
        }
    }
    public class ProductTagEditCommandValidation : ProductTagValidation<ProductTagEditCommand>
    {
        public ProductTagEditCommandValidation(IProductTagRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStatus();
            ValidateName();
        }
    }

    public class ProductTagDeleteCommandValidation : ProductTagValidation<ProductTagDeleteCommand>
    {
        public ProductTagDeleteCommandValidation(IProductTagRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
