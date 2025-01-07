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
    public class StoreCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public class StoreAddCommand : StoreCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public StoreAddCommand(
            Guid id,
            string code,
            string name,
            string? description,
            string? address,
            string? phone,
            int? displayOrder,
            Guid? createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Address = address;
            Phone = phone;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IStoreRepository _context)
        {
            ValidationResult = new StoreAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class StoreEditCommand : StoreCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public StoreEditCommand(
             Guid id,
            string code,
            string name,
            string? description,
            string? address,
            string? phone,
            int? displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {

            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Address = address;
            Phone = phone;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IStoreRepository _context)
        {
            ValidationResult = new StoreEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class StoreSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public StoreSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class StoreDeleteCommand : StoreCommand
    {
        public StoreDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IStoreRepository _context)
        {
            ValidationResult = new StoreDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
