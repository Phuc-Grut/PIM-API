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
    public class SpecificationAttributeOptionCommand : Command
    {
        public Guid Id { get; set; }
        public Guid SpecificationAttributeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Alias { get; set; }
        public int DisplayOrder { get; set; }
        public decimal NumberValue { get; set; }
        public int MediaFileId { get; set; }
        public string? Color { get; set; }
    }

    public class SpecificationAttributeOptionAddCommand : SpecificationAttributeOptionCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public SpecificationAttributeOptionAddCommand(
            Guid id,
            Guid specificationAttributeId,
            string name,
            string? alias,
            int displayOrder,
            decimal numberValue,
            int mediaFileId,
            string? color,
            Guid createdBy,
            DateTime? createdDate)
        {
            Id = id;
            SpecificationAttributeId = specificationAttributeId;
            Name = name;
            Alias = alias;
            NumberValue = numberValue;
            MediaFileId = mediaFileId;
            Color = color;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid() 
        {
            ValidationResult = new SpecificationAttributeOptionAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SpecificationAttributeOptionEditCommand : SpecificationAttributeOptionCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public SpecificationAttributeOptionEditCommand(
            Guid id,
            Guid specificationAttributeId,
            string name,
            string? alias,
            int displayOrder,
            decimal numberValue,
            int mediaFileId,
            string? color,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            SpecificationAttributeId = specificationAttributeId;
            Name = name;
            Alias = alias;
            NumberValue = numberValue;
            MediaFileId = mediaFileId;
            Color = color;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(ISpecificationAttributeOptionRepository _context)
        {
            ValidationResult = new SpecificationAttributeOptionEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SpecificationAttributeOptionDeleteCommand : SpecificationAttributeOptionCommand
    {
        public SpecificationAttributeOptionDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ISpecificationAttributeOptionRepository _context)
        {
            ValidationResult = new SpecificationAttributeOptionDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
