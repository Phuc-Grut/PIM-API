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
    public class CategoryCommand : Command
    {
        public CategoryCommand()
        {
        }

        public CategoryCommand(Guid id, string? code, string? name, string? fullName, string? description, string? image, string? web, Guid? parentCategoryId, Guid? groupCategoryId, int status, int displayOrder, string? keywords, string? jsonData)
        {
            Id = id;
            Code = code;
            Name = name;
            FullName = fullName;
            Description = description;
            Image = image;
            Web = web;
            ParentCategoryId = parentCategoryId;
            GroupCategoryId = groupCategoryId;
            Status = status;
            DisplayOrder = displayOrder;
            Keywords = keywords;
            JsonData = jsonData;
        }

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Web { get; set; } 
        public string Url { get; set; } 
        public Guid? ParentCategoryId { get; set; }
        public Guid? GroupCategoryId { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public string? Keywords { get; set; }
        public string? JsonData { get; set; }
    }

    public class CategoryAddCommand : CategoryCommand
    {
        public CategoryAddCommand(
            Guid id,
            string code,
            string name,string? fullName,
            string? description, string? image, string? web,
            Guid? parentCategoryId,
            Guid? groupCategoryId,
            int status,
            int displayOrder,
            string? keywords,
            string? jsonData,
            Guid createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name; FullName = fullName; 
            Description = description; 
            Image = image;
            Web = web;
            ParentCategoryId = parentCategoryId;
            GroupCategoryId = groupCategoryId;
            Status = status;
            DisplayOrder = displayOrder;
            Keywords = keywords;
            JsonData = jsonData;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public CategoryAddCommand(
            Guid id,
            string code,
            string name,
            string? description,
            Guid? parentCategoryId,
            Guid? groupCategoryId,
            int status,
            int displayOrder,
            string? keywords,
            string? jsonData,
            Guid createdBy,
            DateTime createdDate,
            string? createdName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            GroupCategoryId = groupCategoryId;
            Status = status;
            DisplayOrder = displayOrder;
            Keywords = keywords;
            JsonData = jsonData;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdName;
        }
        public bool IsValid(ICategoryRepository _context)
        {
            ValidationResult = new CategoryAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CategoryEditCommand : CategoryCommand
    {
        public CategoryEditCommand(
           Guid id,
            string code,
            string name,string? fullName,
            string? description, 
            string? image, string? web,
            Guid? parentCategoryId,
            Guid? groupCategoryId,
            int status,
            int displayOrder,
            string? keywords,
            string? jsonData,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Name = name;
            FullName = fullName;
            Description = description; 
            Image = image;
            Web = web;
            Status = status;
            DisplayOrder = displayOrder;
            Keywords = keywords;
            JsonData = jsonData;
            ParentCategoryId = parentCategoryId;
            GroupCategoryId = groupCategoryId;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public CategoryEditCommand(
           Guid id,
            string code,
            string name,
            string? description,
            Guid? parentCategoryId,
            Guid? groupCategoryId,
            int status,
            int displayOrder,
            string? keywords,
            string? jsonData,
            Guid? updatedBy,
            DateTime? updatedDate,
            string? updatedName)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            DisplayOrder = displayOrder;
            Keywords = keywords;
            JsonData = jsonData;
            ParentCategoryId = parentCategoryId;
            GroupCategoryId = groupCategoryId;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedName;
        }
        public bool IsValid(ICategoryRepository _context)
        {
            ValidationResult = new CategoryEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class CategorySortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public CategorySortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class CategoryDeleteCommand : CategoryCommand
    {
        public CategoryDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(ICategoryRepository _context)
        {
            ValidationResult = new CategoryDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
