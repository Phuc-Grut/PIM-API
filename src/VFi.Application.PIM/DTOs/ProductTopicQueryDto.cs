using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.DTOs
{
    public class ProductTopicQueryDto
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
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public int? Condition { get; set; }
        public int? ProductType { get; set; }
        public int? PageQuery { get; set; }
        public int? SortQuery { get; set; }
    }
}
