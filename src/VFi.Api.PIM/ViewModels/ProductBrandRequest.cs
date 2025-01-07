using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductBrandRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Tags { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        
    }
    public class EditProductBrandRequest : AddProductBrandRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductBrandRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingProductBrandRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
