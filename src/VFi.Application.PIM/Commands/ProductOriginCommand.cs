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
    public class ProductOriginCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductOriginAddCommand : ProductOriginCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public ProductOriginAddCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate,
            string createdName)
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
        public bool IsValid(IProductOriginRepository _context)
        {
            ValidationResult = new ProductOriginAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductOriginEditCommand : ProductOriginCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public ProductOriginEditCommand(
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
        public bool IsValid(IProductOriginRepository _context)
        {
            ValidationResult = new ProductOriginEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductOriginSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductOriginSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ProductOriginDeleteCommand : ProductOriginCommand
    {
        public ProductOriginDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductOriginRepository _context)
        {
            ValidationResult = new ProductOriginDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
