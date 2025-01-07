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
    public class ProductTopicCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public string? Image { get; set; }
        public string? Icon { get; set; }
        public string? Icon2 { get; set; }
        public string? Tags { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public string? Title { get; set; }
        public List<Guid>? ProductTopicPageIds { get; set; }
        public List<string>? ProductTopicPageCodes { get; set; }
    }

    public class ProductTopicAddCommand : ProductTopicCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public ProductTopicAddCommand(
            Guid id,
            string? code,
            string? name,
            string? slug,
            string? description,
            string? keywords,
            string? image,
            string? icon,
            string? icon2,
            string? tags,
            int status,
            int displayOrder,
            string? title,
            Guid createdBy,
            DateTime createdDate,
            string? createdByName,
            List<Guid>? productTopicPageIds,
            List<string>? productTopicPageCodes)
        {
            Id = id;
            Code = code;
            Name = name;
            Slug= slug;
            Keywords= keywords;
            Image = image;
            Icon = icon;
            Icon2 = icon2;
            Tags = tags;
            Title = title;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdByName;
            ProductTopicPageIds = productTopicPageIds;
            ProductTopicPageCodes = productTopicPageCodes;
        }

        public ProductTopicAddCommand()
        {
        }

        public bool IsValid(IProductTopicRepository _context)
        {
            ValidationResult = new ProductTopicAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicEditCommand : ProductTopicCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public ProductTopicEditCommand(
            Guid id,
            string? code,
            string? name,
            string? slug,
            string? description,
            string? keywords,
            string? image,
            string? icon,
            string? icon2,
            string? tags,
            int status,
            int displayOrder,
            string? title,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedByName,
            List<Guid>? productTopicPageIds,
            List<string>? productTopicPageCodes)
        {
            Id = id;
            Code = code;
            Name = name;
            Slug = slug;
            Keywords = keywords;
            Image = image;
            Icon = icon;
            Icon2 = icon2;
            Tags = tags;
            Title = title;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedByName;
            ProductTopicPageIds = productTopicPageIds;
            ProductTopicPageCodes = productTopicPageCodes;
        }

        public ProductTopicEditCommand()
        {
        }

        public bool IsValid(IProductTopicRepository _context)
        {
            ValidationResult = new ProductTopicEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class EditProductTopicSortCommand : ProductTopicCommand
    {
        public IEnumerable<ProductTopicSortDto> List { get; set; }

        public EditProductTopicSortCommand(IEnumerable<ProductTopicSortDto> list)
        {
            List = list;
        }
    }
    public class ProductTopicDeleteCommand : ProductTopicCommand
    {
        public ProductTopicDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTopicRepository _context)
        {
            ValidationResult = new ProductTopicDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductTopicSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }
}
