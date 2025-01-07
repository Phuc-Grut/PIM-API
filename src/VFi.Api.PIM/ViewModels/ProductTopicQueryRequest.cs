using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductTopicQueryRequest
    {
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

    public class EditProductTopicQueryRequest : AddProductTopicQueryRequest
    {
        public Guid Id { get; set; }
    }

    public class ListBoxProductTopicQueryRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$productTopicId")]
        public Guid? ProductTopicId { get; set; }
    }
}
