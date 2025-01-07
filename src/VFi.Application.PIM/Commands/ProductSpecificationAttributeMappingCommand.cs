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
    public class ProductSpecificationAttributeMappingCommand : Command
    {
        public Guid Id { get; set; }
        public Guid SpecificationAttributeId { get; set; }
        public Guid SpecificationAttributeOptionId { get; set; }
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; }

    }

    public class ProductSpecificationAttributeMappingAddCommand : ProductSpecificationAttributeMappingCommand
    {
        public ProductSpecificationAttributeMappingAddCommand(
            Guid id,
            Guid productId,
            Guid specificationAttributeId,
            Guid specificationAttributeOptionId,
            int displayOrder)
        {
            Id = id;
            ProductId = productId;
            SpecificationAttributeId = specificationAttributeId;
            SpecificationAttributeOptionId = specificationAttributeOptionId;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductSpecificationAttributeMappingAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductSpecificationAttributeMappingEditCommand : ProductSpecificationAttributeMappingCommand
    {
        public ProductSpecificationAttributeMappingEditCommand(
            Guid id,
            Guid productId,
            Guid specificationAttributeId,
            Guid specificationAttributeOptionId,
            int displayOrder)
        {
            Id = id;
            ProductId = productId;
            SpecificationAttributeId = specificationAttributeId;
            SpecificationAttributeOptionId = specificationAttributeOptionId;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductSpecificationAttributeMappingEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductSpecificationAttributeMappingDeleteCommand : ProductSpecificationAttributeMappingCommand
    {
        public ProductSpecificationAttributeMappingDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductSpecificationAttributeMappingRepository _context)
        {
            ValidationResult = new ProductSpecificationAttributeMappingDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
