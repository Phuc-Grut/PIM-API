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

namespace VFi.Application.PIM.Commands
{
    public class UnitCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? NamePlural { get; set; }
        public string? Description { get; set; }
        public string? DisplayLocale { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
        public Guid? GroupUnitId { get; set; }
        public double? Rate { get; set; }
        public int Status { get; set; }
    }

    public class UnitAddCommand : UnitCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public UnitAddCommand(
            Guid id,
            string? code,
            string name,
            string namePlural,
            string? description,
            Guid? groupUnitId,
            double? rate,
            bool isDefault,
            string?  displayLocale,
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
            NamePlural = namePlural;
            GroupUnitId = groupUnitId;
            Rate = rate;
            IsDefault = isDefault;
            DisplayLocale = displayLocale;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IUnitRepository _context)
        {
            ValidationResult = new UnitAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UnitEditCommand : UnitCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public UnitEditCommand(
          Guid id,
            string? code,
            string name,
            string namePlural,
            string? description,
            Guid? groupUnitId,
            double? rate,
            bool isDefault,
            string? displayLocale,
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
            NamePlural = namePlural;
            GroupUnitId = groupUnitId;
            Status = status;
            Rate = rate;
            IsDefault = isDefault;
            DisplayLocale = displayLocale;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IUnitRepository _context)
        {
            ValidationResult = new UnitEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class UnitSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public UnitSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class UnitDeleteCommand : UnitCommand
    {
        public UnitDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IUnitRepository _context)
        {
            ValidationResult = new UnitDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
