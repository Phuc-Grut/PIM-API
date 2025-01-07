using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation;
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
    public class AddProductVariantAttributeValueDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductVariantAttributeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; } = null!;
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public int? DisplayOrder { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }
    public class ProductAttributeMappingCommand : Command
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string? TextPrompt { get; set; }
        public string? CustomData { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public List<AddProductVariantAttributeValueDto>? ListDetail { get; set; }
    }

    public class ProductAttributeMappingAddCommand : ProductAttributeMappingCommand
    {
        public ProductAttributeMappingAddCommand(
            Guid id,
            Guid productId,
            Guid productAttributeId,
            string? textPrompt,
            string? customData,
            bool isRequired,
            int attributeControlTypeId,
            int displayOrder,
            List<AddProductVariantAttributeValueDto>? listDetail
          )
        {
            Id = id;
            ProductId = productId;
            ProductAttributeId = productAttributeId;
            TextPrompt = textPrompt;
            CustomData = customData;
            IsRequired = isRequired;
            AttributeControlTypeId = attributeControlTypeId;
            DisplayOrder = displayOrder;
            ListDetail = listDetail;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductAttributeMappingAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductAttributeMappingEditCommand : ProductAttributeMappingCommand
    {
        public ProductAttributeMappingEditCommand(
                Guid id,
             Guid productId,
            Guid productAttributeId,
            string? textPrompt,
            string? customData,
            bool isRequired,
            int attributeControlTypeId,
            int displayOrder,
            List<AddProductVariantAttributeValueDto>? listDetail)
        {
            Id = id;
            ProductId = productId;
            ProductAttributeId = productAttributeId;
            TextPrompt = textPrompt;
            CustomData = customData;
            IsRequired = isRequired;
            AttributeControlTypeId = attributeControlTypeId;
            DisplayOrder = displayOrder;
            ListDetail = listDetail;
        }
        public bool IsValid(IProductProductAttributeMappingRepository _context)
        {
            ValidationResult = new ProductAttributeMappingEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
      
    }

    public class ProductAttributeMappingDeleteCommand : ProductAttributeMappingCommand
    {
        public ProductAttributeMappingDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductProductAttributeMappingRepository _context)
        {
            ValidationResult = new ProductAttributeMappingDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
