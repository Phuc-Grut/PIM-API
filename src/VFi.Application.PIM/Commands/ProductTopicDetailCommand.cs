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
    public class ProductTopicDetailCommand : Command
    {

        public Guid Id { get; set; }
        public string ProductTopic { get; set; }
        public string Code { get; set; }
        public int Condition { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; } 
        public string Tags { get; set; }
        public DateTime? Exp { get; set; }
        public decimal? BidPrice { get; set; }
        public int? Tax { get; set; }
    }

    public class ProductTopicDetailAddCommand : ProductTopicDetailCommand
    {
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; } 
        public bool IsValid(IProductTopicDetailRepository _context)
        {
            ValidationResult = new ProductTopicDetailAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductTopicDetailEditCommand : ProductTopicDetailCommand
    {
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
         
        public bool IsValid(IProductTopicDetailRepository _context)
        {
            ValidationResult = new ProductTopicDetailEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductTopicDetailSortCommand : Command
    {
        public List<SortItemDto> SortList { get; set; }
        public ProductTopicDetailSortCommand(List<SortItemDto> sortList)
        {
            SortList = sortList;
        }
    }

    public class ProductTopicDetailDeleteCommand : ProductTopicDetailCommand
    {
        public ProductTopicDetailDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductTopicDetailRepository _context)
        {
            ValidationResult = new ProductTopicDetailDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
