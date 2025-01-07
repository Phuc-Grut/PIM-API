using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductTypeCommand : Command
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class ProductTypeAddCommand : ProductTypeCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public ProductTypeAddCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IProductTypeRepository _context)
        {
            ValidationResult = new ProductTypeAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTypeEditCommand : ProductTypeCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }

        public ProductTypeEditCommand(
           Guid id,
            string? code,
            string name,
            string? description,
            int status,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IProductTypeRepository _context)
        {
            ValidationResult = new ProductTypeEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductTypeSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductTypeSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ProductTypeDeleteCommand : ProductTypeCommand
    {
        public ProductTypeDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTypeRepository _context)
        {
            ValidationResult = new ProductTypeDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
