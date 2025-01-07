using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
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
    public class TaxCategoryCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double Rate { get; set; }
        public string? Group { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public int? Type { get; set; }
    }

    public class TaxCategoryAddCommand : TaxCategoryCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public TaxCategoryAddCommand(
            Guid id,
            string? code,
            string name,
            double rate,
            string? group,
            string? description,
            int status,
            int displayOrder,
            int? type,
            Guid createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Rate = rate;
            Group = group;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            Type = type;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(ITaxCategoryRepository _context)
        {
            ValidationResult = new TaxCategoryAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class TaxCategoryEditCommand : TaxCategoryCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public TaxCategoryEditCommand(
           Guid id,
            string? code,
            string name,
            double rate,
            string? group,
            string? description,
            int status,
            int displayOrder,
            int? type,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Rate = rate;
            Group = group;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            Type = type;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(ITaxCategoryRepository _context)
        {
            ValidationResult = new TaxCategoryEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class TaxCategorySortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public TaxCategorySortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class TaxCategoryDeleteCommand : TaxCategoryCommand
    {
        public TaxCategoryDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ITaxCategoryRepository _context)
        {
            ValidationResult = new TaxCategoryDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
