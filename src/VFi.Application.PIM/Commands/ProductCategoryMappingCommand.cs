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
    public class ProductCategoryMappingCommand : Command
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductCategoryMappingAddCommand : ProductCategoryMappingCommand
    {
        public ProductCategoryMappingAddCommand(
            Guid id,
            Guid categoryId,
            Guid productId,
            int displayOrder)
        {
            Id = id;
            CategoryId = categoryId;
            ProductId = productId;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductCategoryMappingAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductCategoryMappingEditCommand : ProductCategoryMappingCommand
    {
        public ProductCategoryMappingEditCommand(
            Guid id,
            Guid categoryId,
            Guid productId,
            int displayOrder)
        {
            Id = id;
            CategoryId = categoryId;
            ProductId = productId;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductCategoryMappingEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
 
    public class ProductCategoryMappingDeleteCommand : ProductCategoryMappingCommand
    {
        public ProductCategoryMappingDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductCategoryMappingRepository _context)
        {
            ValidationResult = new ProductCategoryMappingDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
