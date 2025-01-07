




using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VFi.Application.PIM.Commands
{
    public class SpecificationAttributeCommand : Command
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; } = null!;
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public List<SpecificationAttributeOptionDto>? Option { get; set; }
        public List<DeleteSpecificationAttributeOptionDto>? Delete { get; set; }
    }

    public class SpecificationAttributeAddCommand : SpecificationAttributeCommand
    {
        
        public SpecificationAttributeAddCommand(
            Guid id,
            string? code,
            string name,
            string? alias,
            string? description,
            int status,
            int displayOrder,
            DateTime createdDate,
            Guid? createdBy,
            string? createdByName,
            List<SpecificationAttributeOptionDto>? option
            )
        {
            Id = id;
            Code = code;
            Name = name;
            Alias = alias;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            Option = option;
            CreatedByName = createdByName;
        }
        public bool IsValid()
        {
            ValidationResult = new SpecificationAttributeAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SpecificationAttributeEditCommand : SpecificationAttributeCommand
    {
        public SpecificationAttributeEditCommand(
            Guid id,
            string? code,
            string? name,
            string? alias,
            string? description,
            int status,
            int displayOrder,
            DateTime createdDate,
            DateTime? updatedDate,
            Guid? createdBy,
            Guid? updatedBy,
            string? createdByName,
            string? updatedByName,
            List<SpecificationAttributeOptionDto>? option

            )
        {
            Id = id;
            Code = code;
            Name = name;
            Alias = alias;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            Option = option;
            CreatedByName = createdByName;
            UpdatedByName = updatedByName;
        }
        public bool IsValid(ISpecificationAttributeRepository _context)
        {
            ValidationResult = new SpecificationAttributeEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class SpecificationAttributeSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public SpecificationAttributeSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class SpecificationAttributeDeleteCommand : SpecificationAttributeCommand
    {
        public SpecificationAttributeDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ISpecificationAttributeRepository _context)
        {
            ValidationResult = new SpecificationAttributeDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
