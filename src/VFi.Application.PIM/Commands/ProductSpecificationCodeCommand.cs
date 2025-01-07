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
    public class ProductSpecificationCodeCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public bool DuplicateAllowed { get; set; }
        public int Status { get; set; }
        public int DataTypes { get; set; }
        public int DisplayOrder { get; set; }

    }

    public class ProductSpecificationCodeAddCommand : ProductSpecificationCodeCommand
    {
        public ProductSpecificationCodeAddCommand(
            Guid id,
            Guid productId,
            string name,
            bool duplicateAllowed,
            int status,
            int dataTypes,
            int displayOrder)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            DuplicateAllowed = duplicateAllowed;
            Status = status;
            DataTypes = dataTypes;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductSpecificationCodeAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductSpecificationCodeEditCommand : ProductSpecificationCodeCommand
    {
        public ProductSpecificationCodeEditCommand(
            Guid id,
            Guid productId,
            string name,
            bool duplicateAllowed,
            int status,
            int dataTypes,
            int displayOrder)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            DuplicateAllowed = duplicateAllowed;
            Status = status;
            DataTypes = dataTypes;
            DisplayOrder = displayOrder;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductSpecificationCodeEditCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductSpecificationCodeDeleteCommand : ProductSpecificationCodeCommand
    {
        public ProductSpecificationCodeDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductSpecificationCodeRepository _context)
        {
            ValidationResult = new ProductSpecificationCodeDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
