using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VFi.Application.PIM.Commands
{
    public class ProductAttributeCommand : Command
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public bool AllowFiltering { get; set; }
        public int? SearchType { get; set; }
        public bool? IsOption { get; set; }
        public int DisplayOrder { get; set; }
        public string? Mapping { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public int? Status { get; set; }
        public List<ProductAttributeOptionDto>? Option { get; set; }
        public List<DeleteProductAttributeOptionDto>? Delete { get; set; }
    }

    public class ProductAttributeAddCommand : ProductAttributeCommand
    {
        public ProductAttributeAddCommand(
            Guid id,
            string code,
            string name,
            string? description,
            string? alias,
            bool allowFiltering,
            int? searchType,
            bool? isOption,
            int displayOrder,
            string? mapping,
            DateTime createdDate,
            Guid? createdBy,
            string? createdByName,
            int? status,
            List<ProductAttributeOptionDto>? option
            )
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Alias = alias;
            AllowFiltering = allowFiltering;
            SearchType = searchType;
            IsOption = isOption;
            DisplayOrder = displayOrder;
            Mapping = mapping;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdByName;
            Status = status;
            Option = option;
        }
        public bool IsValid()
        {
            ValidationResult = new ProductAttributeAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductAttributeEditCommand : ProductAttributeCommand
    {
        public ProductAttributeEditCommand(
            Guid id,
            string? code,
            string name,
            string? description,
            string? alias,
            bool allowFiltering,
            int? searchType,
            bool? isOption,
            int displayOrder,
            string? mapping,
            DateTime createdDate,
            DateTime? updatedDate,
            Guid? createdBy,
            Guid? updatedBy,
            string? createdByName,
            string? updatedByName,
            int? status,
            List<ProductAttributeOptionDto>? option
            )
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Alias = alias;
            AllowFiltering = allowFiltering;
            SearchType = searchType;
            IsOption = isOption;
            DisplayOrder = displayOrder;
            Mapping = mapping;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedByName = createdByName;
            UpdatedByName = updatedByName;
            Status = status;
            Option = option;
        }
        public bool IsValid(IProductAttributeRepository _context)
        {
            ValidationResult = new ProductAttributeEditCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductAttributeSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductAttributeSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ProductAttributeDeleteCommand : ProductAttributeCommand
    {
        public ProductAttributeDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductAttributeRepository _context)
        {
            ValidationResult = new ProductAttributeDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
