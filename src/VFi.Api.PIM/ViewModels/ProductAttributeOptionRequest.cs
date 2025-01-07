using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductAttributeOptionRequest
    {
        public Guid? Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public bool IsPreSelected { get; set; }
        public int DisplayOrder { get; set; }
        public int? ValueTypeId { get; set; }
        public Guid? LinkedProductId { get; set; }
        public int? Quantity { get; set; }
    }
    public class EditProductAttributeOptionRequest : AddProductAttributeOptionRequest
    {
        public string Id { get; set; }
    }

    public class DeleteProductAttributeOptionRequest
    {
        public Guid? Id { get; set; }
    }
    public class ListBoxProductAttributeOptionRequest : ListBoxRequest
    {
        [FromQuery(Name = "$productAttributeId")]
        public string? ProductAttributeId { get; set; }
    }
    public class PagingProductAttributeRequest : PagingRequest
    {
        [FromQuery(Name = "$productAttributeId")]
        public string? ProductAttributeId { get; set; }
    }
}
