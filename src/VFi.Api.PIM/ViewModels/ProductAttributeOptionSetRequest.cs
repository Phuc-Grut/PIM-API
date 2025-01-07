using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductAttributeOptionSetRequest
    {
        public string? Name { get; set; }
        public string ProductAttributeId { get; set; } = null!;
    }
    public class EditProductAttributeOptionSetRequest : AddProductAttributeOptionSetRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductAttributeOptionSetRequest : ListBoxRequest
    {
        [FromQuery(Name = "$productAttributeId")]
        public string? ProductAttributeId { get; set; }
    }
    public class PagingProductAttributeOptionSetRequest : PagingRequest
    {
        [FromQuery(Name = "$productAttributeId")]
        public string? ProductAttributeId { get; set; }
    }
}
