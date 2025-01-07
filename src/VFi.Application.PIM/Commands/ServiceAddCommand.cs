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
    public class ServiceAddCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CalculationMethod { get; set; }
        public decimal? Price { get; set; }
        public string? PriceSyntax { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public string? Currency { get; set; }
    }

    public class ServiceAddAddCommand : ServiceAddCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }

        public ServiceAddAddCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            int calculationMethod,
            decimal? price,
            string? priceSyntax,
            decimal? minPrice,
            decimal? maxPrice,
            int status,
            int displayOrder,
            string? currency,
            Guid? createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            CalculationMethod = calculationMethod;
            Price = price;
            PriceSyntax = priceSyntax;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Status = status;
            DisplayOrder = displayOrder;
            Currency = currency;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IServiceAddRepository _context)
        {
            ValidationResult = new ServiceAddAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ServiceAddEditCommand : ServiceAddCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public ServiceAddEditCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            int calculationMethod,
            decimal? price,
            string? priceSyntax,
            decimal? minPrice,
            decimal? maxPrice,
            int status,
            int displayOrder,
            string? currency,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {

            Id = id;
            Code = code;
            Name = name;
            Description = description;
            CalculationMethod = calculationMethod;
            Price = price;
            PriceSyntax = priceSyntax;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            Status = status;
            DisplayOrder = displayOrder;
            Currency = currency;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IServiceAddRepository _context)
        {
            ValidationResult = new ServiceAddEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ServiceAddSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ServiceAddSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ServiceAddDeleteCommand : ServiceAddCommand
    {
        public ServiceAddDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IServiceAddRepository _context)
        {
            ValidationResult = new ServiceAddDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
