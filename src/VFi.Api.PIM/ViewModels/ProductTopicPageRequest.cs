using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductTopicPageRequest
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public string? Slug { get; set; }
        public string? Image { get; set; }
        public string? Icon { get; set; }
        public string? Icon2 { get; set; }
        public string? Tags { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditProductTopicPageRequest : AddProductTopicPageRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductTopicPageRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingProductTopicPageRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class ProductTopicPageSort
    {
        public Guid Id { get; set; }
        public int? SortOrder { get; set; }
    }

    public class EditProductTopicPageSortRequest
    {
        public List<ProductTopicPageSort> ListGuid { get; set; }
    }

   
}
