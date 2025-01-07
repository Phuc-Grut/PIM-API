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
    public class ProductTopicPageCommand : Command
    {

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Keywords { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Icon { get; set; }
        public string? Icon2 { get; set; } 
        public string? Tags { get; set; } 
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductTopicPageAddCommand : ProductTopicPageCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public ProductTopicPageAddCommand(
            Guid id,
            string code,
            string? name,
            string? description,
            int status,
            int displayOrder,
            Guid createdBy,
            DateTime createdDate,
            string? createdByName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdByName;
        }

        public ProductTopicPageAddCommand()
        {
        }

        public bool IsValid(IProductTopicPageRepository _context)
        {
            ValidationResult = new ProductTopicPageAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicPageEditCommand : ProductTopicPageCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public ProductTopicPageEditCommand(
           Guid id,
            string code,
            string? name,
            string? description,
            int status,
            int displayOrder,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedByName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedByName;
        }

        public ProductTopicPageEditCommand()
        {
        }

        public bool IsValid(IProductTopicPageRepository _context)
        {
            ValidationResult = new ProductTopicPageEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class EditProductTopicPageSortCommand : ProductTopicPageCommand
    {
        public IEnumerable<ProductTopicPageSortDto> List { get; set; }

        public EditProductTopicPageSortCommand(IEnumerable<ProductTopicPageSortDto> list)
        {
            List = list;
        }
    }
    public class ProductTopicPageDeleteCommand : ProductTopicPageCommand
    {
        public ProductTopicPageDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTopicPageRepository _context)
        {
            ValidationResult = new ProductTopicPageDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicPageSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductTopicPageSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }
}
