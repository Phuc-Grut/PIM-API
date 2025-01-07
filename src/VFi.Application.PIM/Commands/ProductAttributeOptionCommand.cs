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
    public class ProductAttributeOptionCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public bool IsPreSelected { get; set; }
        public int DisplayOrder { get; set; }
        public int? ValueTypeId { get; set; }
        public Guid LinkedProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }

    public class ProductAttributeOptionAddCommand : ProductAttributeOptionCommand
    {
        public ProductAttributeOptionAddCommand(
            Guid id,
            Guid productAttributeId,
            string? name,
            string? alias,
            string? image,
            string? color,
            decimal priceAdjustment,
            decimal weightAdjustment,
            bool isPreSelected,
            int displayOrder,
            int? valueTypeId,
            Guid linkedProductId,
            int? quantity,
            DateTime createdDate,
            Guid? createdBy
            )
        {
            Id = id;
            ProductAttributeId = productAttributeId;
            Name = name;
            Alias = alias;
            Image = image;
            Color = color;
            PriceAdjustment = priceAdjustment;
            WeightAdjustment = weightAdjustment;
            IsPreSelected = isPreSelected;
            DisplayOrder = displayOrder;
            ValueTypeId = valueTypeId;
            LinkedProductId = linkedProductId;
            Quantity = quantity;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductAttributeOptionAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductAttributeOptionEditCommand : ProductAttributeOptionCommand
    {
        public ProductAttributeOptionEditCommand(
            Guid id,
            Guid productAttributeId,
            string? name,
            string? alias,
            string? image,
            string? color,
            decimal priceAdjustment,
            decimal weightAdjustment,
            bool isPreSelected,
            int displayOrder,
            int? valueTypeId,
            Guid linkedProductId,
            int? quantity,
            DateTime? updatedDate,
            Guid? updatedBy)
        {
            Id = id;
            ProductAttributeId = productAttributeId;
            Name = name;
            Alias = alias;
            Image = image;
            Color = color;
            PriceAdjustment = priceAdjustment;
            WeightAdjustment = weightAdjustment;
            IsPreSelected = isPreSelected;
            DisplayOrder = displayOrder;
            ValueTypeId = valueTypeId;
            LinkedProductId = linkedProductId;
            Quantity = quantity;
            UpdatedDate = updatedDate;
            UpdatedBy = updatedBy;
        }
        public bool IsValid(IProductAttributeOptionRepository _context)
        {
            ValidationResult = new ProductAttributeOptionEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductAttributeOptionDeleteCommand : ProductAttributeOptionCommand
    {
        public ProductAttributeOptionDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductAttributeOptionRepository _context)
        {
            ValidationResult = new ProductAttributeOptionDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
