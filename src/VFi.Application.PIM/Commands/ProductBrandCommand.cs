using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductBrandCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Tags { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
       
    }

    public class ProductBrandAddCommand : ProductBrandCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public ProductBrandAddCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            string? image,
            string? tags,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Image = image;
            Tags = tags;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(IProductBrandRepository _context)
        {
            ValidationResult = new ProductBrandAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductBrandEditCommand : ProductBrandCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public ProductBrandEditCommand(
           Guid id,
            string? code,
            string name,
            string? description,
            string? image,
            string? tags,
            int status,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Image = image;
            Tags = tags;
            Status = status;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(IProductBrandRepository _context)
        {
            ValidationResult = new ProductBrandEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductBrandSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductBrandSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ProductBrandDeleteCommand : ProductBrandCommand
    {
        public ProductBrandDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductBrandRepository _context)
        {
            ValidationResult = new ProductBrandDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
