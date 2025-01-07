using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductVariantAttributeCombinationCommand : Command
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid ProductId { get; set; }
        public string? Sku { get; set; }
        public string? Gtin { get; set; }
        public string? ManufacturerPartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? BasePriceAmount { get; set; }
        public int? BasePriceBaseAmount { get; set; }
        public string? AssignedMediaFileIds { get; set; }
        public bool IsActive { get; set; }
        public Guid? DeliveryTimeId { get; set; }
        public Guid? QuantityUnitId { get; set; }
        public string? AttributesXml { get; set; }
        public int StockQuantity { get; set; }
        public bool AllowOutOfStockOrders { get; set; }
    }

    public class ProductVariantAttributeCombinationAddCommand : ProductVariantAttributeCombinationCommand
    {
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public ProductVariantAttributeCombinationAddCommand(Guid id,string? name,Guid productId, string? sku, string? gtin, string? manufacturerPartNumber, decimal? price, decimal? length, decimal? width, decimal? height, decimal? basePriceAmount, int? basePriceBaseAmount, string? assignedMediaFileIds, bool isActive, Guid? deliveryTimeId, Guid? quantityUnitId, string? attributesXml, int stockQuantity, bool allowOutOfStockOrders, DateTime createdDate, Guid? createdBy)
        {
            Id = id;
            Name = name;
            ProductId = productId;
            Sku = sku;
            Gtin = gtin;
            ManufacturerPartNumber = manufacturerPartNumber;
            Price = price;
            Length = length;
            Width = width;
            Height = height;
            BasePriceAmount = basePriceAmount;
            BasePriceBaseAmount = basePriceBaseAmount;
            AssignedMediaFileIds = assignedMediaFileIds;
            IsActive = isActive;
            DeliveryTimeId = deliveryTimeId;
            QuantityUnitId = quantityUnitId;
            AttributesXml = attributesXml;
            StockQuantity = stockQuantity;
            AllowOutOfStockOrders = allowOutOfStockOrders;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductVariantAttributeCombinationAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductVariantAttributeCombinationEditCommand : ProductVariantAttributeCombinationCommand
    {
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public ProductVariantAttributeCombinationEditCommand(Guid id,string? name, Guid productId, string? sku, string? gtin, string? manufacturerPartNumber, decimal? price, decimal? length, decimal? width, decimal? height, decimal? basePriceAmount, int? basePriceBaseAmount, string? assignedMediaFileIds, bool isActive, Guid? deliveryTimeId, Guid? quantityUnitId, string? attributesXml, int stockQuantity, bool allowOutOfStockOrders, DateTime updatedDate, Guid updatedBy)
        {
            Id = id;
            Name = name;
            ProductId = productId;
            Sku = sku;
            Gtin = gtin;
            ManufacturerPartNumber = manufacturerPartNumber;
            Price = price;
            Length = length;
            Width = width;
            Height = height;
            BasePriceAmount = basePriceAmount;
            BasePriceBaseAmount = basePriceBaseAmount;
            AssignedMediaFileIds = assignedMediaFileIds;
            IsActive= isActive;
            DeliveryTimeId = deliveryTimeId;
            QuantityUnitId = quantityUnitId;
            AttributesXml = attributesXml;
            StockQuantity = stockQuantity;
            AllowOutOfStockOrders = allowOutOfStockOrders;
            UpdatedDate = updatedDate;
            UpdatedBy = updatedBy;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductVariantAttributeCombinationEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductVariantAttributeCombinationDeleteCommand : ProductVariantAttributeCombinationCommand
    {
        public ProductVariantAttributeCombinationDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductVariantAttributeCombinationRepository _context)
        {
            ValidationResult = new ProductVariantAttributeCombinationDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
