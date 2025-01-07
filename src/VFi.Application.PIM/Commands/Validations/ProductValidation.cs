using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using static VFi.Application.PIM.Commands.ProductSizeEditCommand;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class ProductValidation<T> : AbstractValidator<T> where T : ProductCommand

    {
        protected readonly IProductRepository _context;
        private Guid Id;
        public ProductValidation(IProductRepository context)
        {
            _context = context;
        }
        public ProductValidation(IProductRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        protected void ValidateAddCodeUnique()
        {
            RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("ProductCode AlreadyExists");
        }
        private bool IsAddUnique(string code)
        {
            var model = _context.GetByCode(code).Result;

            if (model == null)
            {
                return true;
            }

            return false;
        }
        private bool IsEditUnique(string code)
        {
            var model = _context.GetByCode(code).Result;

            if (model == null || model.Id == Id)
            {
                return true;
            }

            return false;
        }
        protected void ValidateEditCodeUnique()
        {
            RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("ProductCode AlreadyExists");
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
                .NotEqual(Guid.Empty).WithMessage("Id IsRequired");
        }
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name IsRequired")
                .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
        }
     
    }
    public abstract class ProductVariantValidation<T> : AbstractValidator<T> where T : ProductVariantAddCommand
    {
        protected readonly IProductRepository _context;
        public ProductVariantValidation(IProductRepository context)
        {
            _context = context;
        }
        private bool checkExistAttrJson(string? attrJson, Guid parentId)
        {
            var item = _context.CheckExistAttrJson(attrJson, parentId).Result;
            if (item == null)
            {
                return true;
            } else
            {
                return false;
            }
        }
        protected void ValidatAttrJsonUnique()
        {
            RuleFor(x => new {x.AttributesJson, x.ParentId}).Must(x=> checkExistAttrJson(x.AttributesJson, x.ParentId)).WithMessage("AttributeAlreadyExist");
        }
     
    }
    public class ProductVariantAddCommandValidation : ProductVariantValidation<ProductVariantAddCommand>
    {
        public ProductVariantAddCommandValidation(IProductRepository context) : base(context)
        {
            ValidatAttrJsonUnique();
        }
    }
    public class ProductAddCommandValidation : ProductValidation<ProductAddCommand>
    {
        public ProductAddCommandValidation(IProductRepository context) : base(context)
        {
            ValidateId();
            ValidateAddCodeUnique();
            ValidateName();
        }
    }
    public class ProductAddCompactCommandValidation : ProductValidation<ProductAddCompactCommand>
    {
        public ProductAddCompactCommandValidation(IProductRepository context) : base(context)
        {
            ValidateId();
            ValidateAddCodeUnique();
            ValidateName();
        }
    }
    public class ProductEditCommandValidation : ProductValidation<ProductEditCommand>
    {
        public ProductEditCommandValidation(IProductRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateName();
        }
    }
    public class ProductSizeEditCommandValidation : ProductValidation<ProductSizeEditCommand>
    {
        public ProductSizeEditCommandValidation(IProductRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
        }
    }
    public class ProductDuplicateCommandValidation : ProductValidation<ProductDuplicateCommand>
    {
        public ProductDuplicateCommandValidation(IProductRepository context) : base(context)
        {
            ValidateId();
            ValidateAddCodeUnique();
            ValidateName();
        }
    }

    public class ProductDeleteCommandValidation : ProductValidation<ProductDeleteCommand>
    {
        public ProductDeleteCommandValidation(IProductRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }
 
    public class ProductCrossValidation : AbstractValidator<ProductCrossCommand>
    {

        protected readonly IProductRepository _context;
        private Guid Id;
        public ProductCrossValidation(IProductRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }

    }

}
