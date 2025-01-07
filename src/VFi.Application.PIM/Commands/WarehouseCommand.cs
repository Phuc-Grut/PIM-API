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
    public class WarehouseCommand : Command
    {

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Company { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Api { get; set; }
        public string? Token { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Status { get; set; }
    }

    public class WarehouseAddCommand : WarehouseCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public WarehouseAddCommand(
            Guid id,
            string code,
            string name,
            double?  latitude,
            double?  longitude,
            string? company ,
            string? country ,
            string? province,
            string? district,
            string? ward,
            string? address,
            string? postalCode,
            string? phoneNumber,
            string? api,
            string? token,
            int? displayOrder,
            int? status,
            Guid? createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Company = company;
            Country = country;
            Province = province;
            District = district;
            Ward = ward;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Api = api;
            Token = token;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IWarehouseRepository _context)
        {
            ValidationResult = new WarehouseAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class WarehouseEditCommand : WarehouseCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public WarehouseEditCommand(
            Guid id,
            string code,
            string name,
            double? latitude,
            double? longitude,
            string? company,
            string? country,
            string? province,
            string? district,
            string? ward,
            string? address,
            string? postalCode,
            string? phoneNumber,
            string? api,
            string? token,
            int? displayOrder,
            int? status,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Company = company;
            Country = country;
            Province = province;
            District = district;
            Ward = ward;
            Address = address;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            Api = api;
            Token = token;
            DisplayOrder = displayOrder;
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IWarehouseRepository _context)
        {
            ValidationResult = new WarehouseEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class WarehouseSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public WarehouseSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class WarehouseDeleteCommand : WarehouseCommand
    {
        public WarehouseDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IWarehouseRepository _context)
        {
            ValidationResult = new WarehouseDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
