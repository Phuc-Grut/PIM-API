using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductTopicRequest
    {
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
    public class EditProductTopicRequest : AddProductTopicRequest
    {
        public Guid Id { get; set; }
    }
    public class ListBoxProductTopicRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$productTopicPageId")]
        public Guid? ProductTopicPageId { get; set; }
    }
    public class PagingProductTopicRequest : FilterQuery
    {
        public Guid? ProductTopicPageId { get; set; }
    }
    public class ProductTopicSort
    {
        public Guid Id { get; set; }
        public int? SortOrder { get; set; }
    }

    public class EditProductTopicSortRequest
    {
        public List<ProductTopicSort> ListGuid { get; set; }
    }

   
}
