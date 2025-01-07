using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class RelatedProductValidation<T> : AbstractValidator<T> where T : RelatedProductCommand

    {
        protected readonly IRelatedProductRepository _context;
        public RelatedProductValidation(IRelatedProductRepository context)
        {
            _context = context;
        }
        public RelatedProductValidation()
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
        protected void ValidateProductId1()
        {
            RuleFor(c => c.ProductId1)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Product");
        }  protected void ValidateProductId2()
        {
            RuleFor(c => c.ProductId2)
                .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the related Product");
        }
       
    }
    public class RelatedProductAddCommandValidation : RelatedProductValidation<RelatedProductAddCommand>
    {
        public RelatedProductAddCommandValidation()
        {
            ValidateId();
            ValidateProductId1();
            ValidateProductId2();
        }
    }
    public class RelatedProductEditCommandValidation : RelatedProductValidation<RelatedProductEditCommand>
    {
        public RelatedProductEditCommandValidation(IRelatedProductRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
            ValidateProductId1();
            ValidateProductId2();
        }
    }

    public class RelatedProductDeleteCommandValidation : RelatedProductValidation<RelatedProductDeleteCommand>
    {
        public RelatedProductDeleteCommandValidation(IRelatedProductRepository _context) : base(_context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
