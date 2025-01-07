﻿using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace VFi.Application.PIM.Commands.Validations
{
    public abstract class CategoryValidation<T> : AbstractValidator<T> where T : CategoryCommand

    {
        protected readonly ICategoryRepository _context;
        private Guid Id;
        public CategoryValidation(ICategoryRepository context, Guid id)
        {
            _context = context;
            Id = id;
        }
        public CategoryValidation(ICategoryRepository context)
        {
            _context = context;
        }

        private bool IsAddUnique(string? code)
        {
            return !_context.CheckExist(code, null).Result;
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
        protected void ValidateDisplayOrder()
        {
            RuleFor(c => c.DisplayOrder)
                      .NotNull().WithMessage("Please ensure you have entered the displayOrder");
        }
     
    }
    public class CategoryAddCommandValidation : CategoryValidation<CategoryAddCommand>
    {
        public CategoryAddCommandValidation(ICategoryRepository context) : base(context)
        {
            ValidateId();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
        }
    }
    public class CategoryEditCommandValidation : CategoryValidation<CategoryEditCommand>
    {
        public CategoryEditCommandValidation(ICategoryRepository context, Guid id) : base(context, id)
        {
            ValidateId();
            ValidateIdExists();
            ValidateStatus();
            ValidateDisplayOrder();
            ValidateCode();
            ValidateName();
        }
    }

    public class CategoryDeleteCommandValidation : CategoryValidation<CategoryDeleteCommand>
    {
        public CategoryDeleteCommandValidation(ICategoryRepository context) : base(context)
        {
            ValidateId();
            ValidateIdExists();
        }
    }

}
