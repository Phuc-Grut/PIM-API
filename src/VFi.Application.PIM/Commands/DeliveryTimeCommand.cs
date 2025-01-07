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
    public class DeliveryTimeCommand : Command
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public int? Status { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class DeliveryTimeAddCommand : DeliveryTimeCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public DeliveryTimeAddCommand(
            Guid id,
            string name,
            bool? isDefault,
            int? minDays,
            int? maxDays,
            int displayOrder,
            int? status,
            Guid createdBy,
            DateTime? createdDate,
            string? createdName
            )
        {
            Id = id;
            IsDefault = isDefault;
            Name = name;
            MinDays = minDays;
            MaxDays = maxDays;
            DisplayOrder = displayOrder;
            Status = status;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid() 
        {
            ValidationResult = new DeliveryTimeAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeliveryTimeEditCommand : DeliveryTimeCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public DeliveryTimeEditCommand(
            Guid id,
            string name,
            bool? isDefault,
            int? minDays,
            int? maxDays,
            int displayOrder,
            int? status,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            IsDefault = isDefault;
            Name = name;
            MinDays = minDays;
            MaxDays = maxDays;
            DisplayOrder = displayOrder;
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IDeliveryTimeRepository _context)
        {
            ValidationResult = new DeliveryTimeEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class DeliveryTimeSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public DeliveryTimeSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class DeliveryTimeDeleteCommand : DeliveryTimeCommand
    {
        public DeliveryTimeDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IDeliveryTimeRepository _context)
        {
            ValidationResult = new DeliveryTimeDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
