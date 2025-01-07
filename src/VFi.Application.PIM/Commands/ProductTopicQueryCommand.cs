using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.NetDevPack.Messaging;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductTopicQueryCommand: Command
    {
        public Guid Id { get; set; }
        public Guid ProductTopicId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? SourceCode { get; set; }
        public string? SourcePath { get; set; }
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public string? Seller { get; set; }
        public string? BrandId { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public int? Condition { get; set; }
        public int? ProductType { get; set; }
        public int? PageQuery { get; set; }
        public int? SortQuery { get; set; }
    }

    public class AddProductTopicQueryCommand : ProductTopicQueryCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }

        public AddProductTopicQueryCommand(Guid id,
                                            Guid productTopicId,
                                            string? name,
                                            string? title,
                                            string? description,
                                            string? sourceCode,
                                            string? sourcePath,
                                            string? keyword,
                                            string? category,
                                            string? seller,
                                            string? brandId,
                                            int status,
                                            int displayOrder,
                                            int? condition,
                                            int? roductType,
                                            int? pageQuery,
                                            int? sortQuery,
                                            Guid? createdBy,
                                            DateTime createdDate,
                                            string? createdByName) 
        {
            Id = id;
            ProductTopicId = productTopicId;
            Name = name;
            Title = title;
            Description = description;
            SourceCode = sourceCode;
            SourcePath = sourcePath;
            Keyword = keyword;
            Category = category;
            Seller = seller;
            BrandId = brandId;
            Status = status;
            DisplayOrder = displayOrder;
            Condition = condition;
            ProductType = roductType;
            PageQuery = pageQuery;
            SortQuery = sortQuery;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            CreatedByName = createdByName;
        }

        public bool IsValid(IProductTopicQueryRepository _context)
        {
            ValidationResult = new ProductTopicQueryAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class EditProductTopicQueryCommand : ProductTopicQueryCommand
    {
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }

        public EditProductTopicQueryCommand(Guid id,
                                            Guid productTopicId,
                                            string? name,
                                            string? title,
                                            string? description,
                                            string? sourceCode,
                                            string? sourcePath,
                                            string? keyword,
                                            string? category,
                                            string? seller,
                                            string? brandId,
                                            int status,
                                            int displayOrder,
                                            int? condition,
                                            int? roductType,
                                            int? pageQuery,
                                            int? sortQuery,
                                            Guid? updatedBy,
                                            DateTime updatedDate,
                                            string? updatedByName)
        {
            Id = id;
            ProductTopicId = productTopicId;
            Name = name;
            Title = title;
            Description = description;
            SourceCode = sourceCode;
            SourcePath = sourcePath;
            Keyword = keyword;
            Category = category;
            Seller = seller;
            BrandId = brandId;
            Status = status;
            DisplayOrder = displayOrder;
            Condition = condition;
            ProductType = roductType;
            PageQuery = pageQuery;
            SortQuery = sortQuery;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
            UpdatedByName = updatedByName;
        }

        public bool IsValid(IProductTopicQueryRepository _context)
        {
            ValidationResult = new ProductTopicQueryEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteProductTopicQueryCommand : ProductTopicQueryCommand
    {
        public DeleteProductTopicQueryCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTopicQueryRepository _context)
        {
            ValidationResult = new ProductTopicQueryDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicQuerySortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductTopicQuerySortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }
}
