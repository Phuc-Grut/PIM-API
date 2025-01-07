using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
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
    public class ProductInventoryCommand : Command
    {

        public Guid Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? ProductId { get; set; }
        public int? StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public int? PlannedQuantity { get; set; }
        public List<ProductInventoryDto>? ListInventory { get; set; }
    }
    public class ProductInventoryMultiCommand : Command
    {

        public Guid? ProductId { get; set; }
        public List<ProductInventoryDto>? ListInventory { get; set; }
    }

    public class ProductInventoryAddCommand : ProductInventoryCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductInventoryAddCommand(
            Guid id,
            Guid? warehouseId,
            Guid? productId,
            int? stockQuantity,
            int? plannedQuantity,
            int? reservedQuantity,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            WarehouseId= warehouseId;
            ProductId = productId;
            StockQuantity = stockQuantity;
            ReservedQuantity = reservedQuantity;
            PlannedQuantity= plannedQuantity;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductInventoryAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductInventoryEditCommand : ProductInventoryCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductInventoryEditCommand(
            Guid id,
            Guid? warehouseId,
            Guid? productId,
            int? stockQuantity,
            int? plannedQuantity,
            int? reservedQuantity,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            WarehouseId = warehouseId;
            ProductId = productId;
            StockQuantity = stockQuantity;
            ReservedQuantity = reservedQuantity;
            PlannedQuantity = plannedQuantity;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(IProductInventoryRepository _context)
        {
            ValidationResult = new ProductInventoryEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductInventoryAddListCommand : ProductInventoryMultiCommand
    {
        public ProductInventoryAddListCommand(
               Guid? productId,
          List<ProductInventoryDto>? list
           )
        {
            ProductId = productId;
            ListInventory = list;
        }
    }
    public class ProductInventoryDeleteCommand : ProductInventoryCommand
    {
        public ProductInventoryDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductInventoryRepository _context)
        {
            ValidationResult = new ProductInventoryDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
