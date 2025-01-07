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
    public class ProductPackageCommand : Command
    {

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
    }

    public class ProductPackageAddCommand : ProductPackageCommand
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductPackageAddCommand(
            Guid id,
            Guid productId,
            string? name,
            decimal? weight,
            decimal? length,
            decimal? width,
            decimal? height,
            Guid createdBy,
            DateTime createdDate)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductPackageAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductPackageEditCommand : ProductPackageCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProductPackageEditCommand(
            Guid id,
            Guid productId,
            string? name,
            decimal? weight,
            decimal? length,
            decimal? width,
            decimal? height,
            Guid? updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            ProductId = productId;
            Name = name;
            Weight = weight;
            Length = length;
            Width = width;
            Height = height;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }
        public bool IsValid(IProductPackageRepository _context)
        {
            ValidationResult = new ProductPackageEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductPackageDeleteCommand : ProductPackageCommand
    {
        public ProductPackageDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductPackageRepository _context)
        {
            ValidationResult = new ProductPackageDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
