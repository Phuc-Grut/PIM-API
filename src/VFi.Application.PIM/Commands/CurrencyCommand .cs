using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class CurrencyCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Locale { get; set; }
        public string? CustomFormatting { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class CurrencyAddCommand : CurrencyCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public CurrencyAddCommand(
            Guid id,
            string? code,
            string? name,
            string? locale,
            string? customFormatting,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime? createdDate)
        {
            Id = id;
            Code = code;
            Name = name;
            Locale = locale;
            CustomFormatting = customFormatting;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid(ICurrencyRepository _context)
        {
            ValidationResult = new CurrencyAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CurrencyEditCommand : CurrencyCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public CurrencyEditCommand(
            Guid id,
            string? code,
            string? name,
            string? locale,
            string? customFormatting,
            int status,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            Code = code;
            Name = name;
            Locale = locale;
            CustomFormatting = customFormatting;
            Status = status;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(ICurrencyRepository _context)
        {
            ValidationResult = new CurrencyEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CurrencyDeleteCommand : CurrencyCommand
    {
        public CurrencyDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ICurrencyRepository _context)
        {
            ValidationResult = new CurrencyDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
