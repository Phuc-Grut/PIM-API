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
    public class ManufacturerCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
    }

    public class ManufacturerAddCommand : ManufacturerCommand
    {
       
        public ManufacturerAddCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate,
            string? createdName
        )
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
        public bool IsValid(IManufacturerRepository _context)
        {
            ValidationResult = new ManufacturerAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ManufacturerEditCommand : ManufacturerCommand
    {
      

        public ManufacturerEditCommand(
           Guid id,
            string? code,
            string name,
            string? description,
            int status,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName
            )
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
        public bool IsValid(IManufacturerRepository _context)
        {
            ValidationResult = new ManufacturerEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ManufacturerSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ManufacturerSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ManufacturerDeleteCommand : ManufacturerCommand
    {
        public ManufacturerDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IManufacturerRepository _context)
        {
            ValidationResult = new ManufacturerDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
