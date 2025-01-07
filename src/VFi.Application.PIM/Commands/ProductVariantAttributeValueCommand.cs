using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VFi.Application.PIM.Commands
{
    public class ProductVariantAttributeValueCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ProductVariantAttributeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; } = null!;
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductVariantAttributeValueAddCommand : ProductVariantAttributeValueCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductVariantAttributeValueAddCommand(
            Guid id,
            Guid productVariantAttributeId,
            string? code,
            string? name,
            string? alias,
            string? image,
            string? color,
            decimal priceAdjustment,
            decimal weightAdjustment,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            ProductVariantAttributeId = productVariantAttributeId;
            Code = code;
            Name = name;
            Alias = alias;
            Image = image;
            Color = color;
            PriceAdjustment = priceAdjustment;
            WeightAdjustment = weightAdjustment;
            DisplayOrder = displayOrder;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductVariantAttributeValueAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductVariantAttributeValueEditCommand : ProductVariantAttributeValueCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductVariantAttributeValueEditCommand(
            Guid id,
            Guid productVariantAttributeId,
            string? code,
            string? name,
            string? alias,
            string? image,
            string? color,
            decimal priceAdjustment,
            decimal weightAdjustment,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            ProductVariantAttributeId = productVariantAttributeId;
            Code = code;
            Name = name;
            Alias = alias;
            Image = image;
            Color = color;
            PriceAdjustment = priceAdjustment;
            WeightAdjustment = weightAdjustment;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(IProductVariantAttributeValueRepository _context)
        {
            ValidationResult = new ProductVariantAttributeValueEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductVariantAttributeValueDeleteCommand : ProductVariantAttributeValueCommand
    {
        public ProductVariantAttributeValueDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductVariantAttributeValueRepository _context)
        {
            ValidationResult = new ProductVariantAttributeValueDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
