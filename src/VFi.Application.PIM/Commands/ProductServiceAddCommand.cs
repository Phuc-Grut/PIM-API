using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using MassTransit.Internals.GraphValidation;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VFi.Application.PIM.Commands
{
    public class ProductServiceAddCommand : Command
    {

        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ServiceAddId { get; set; }
        public int? PayRequired { get; set; }
        public decimal? Price { get; set; }
        public decimal? MaxPrice { get; set; }
        public int CalculationMethod { get; set; }
        public string? PriceSyntax { get; set; }
        public decimal? MinPrice { get; set; }
        public string? Currency { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ProductServiceAddAddCommand : ProductServiceAddCommand
    {
        public ProductServiceAddAddCommand(
            Guid id,
            Guid? productId,
            Guid? serviceAddId,
            int? payRequired,
            decimal? price,
            decimal? maxPrice,
            int calculationMethod,
            string? priceSyntax,
            decimal? minPrice,  
            string? currency,  
            int? status,
            Guid? createdBy,
            DateTime createdDate)
        {
            Id = id;
            ProductId = productId;
            ServiceAddId = serviceAddId;
            PayRequired = payRequired;
            Price = price;
            MaxPrice = maxPrice;
            CalculationMethod = calculationMethod;
            PriceSyntax = priceSyntax;
            MinPrice = minPrice;
            Currency = currency;
            Status = status;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductServiceAddAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductServiceAddEditCommand : ProductServiceAddCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductServiceAddEditCommand(
            Guid id,
            Guid? productId,
            Guid? serviceAddId,
             int? payRequired,
            decimal? price,
            decimal? maxPrice,
            int calculationMethod,
            string? priceSyntax,
            decimal? minPrice,
            string? currency,
            int? status,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            ProductId = productId;
            ServiceAddId = serviceAddId;
            PayRequired = payRequired;
            Price = price;
            MaxPrice = maxPrice;
            CalculationMethod = calculationMethod;
            PriceSyntax = priceSyntax;
            MinPrice = minPrice;
            Currency = currency;
            Status = status;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(IProductServiceAddRepository _context)
        {
            ValidationResult = new ProductServiceAddEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductServiceAddDeleteCommand : ProductServiceAddCommand
    {
        public ProductServiceAddDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductServiceAddRepository _context)
        {
            ValidationResult = new ProductServiceAddDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
