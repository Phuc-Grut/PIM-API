using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductVariantAttributeValueRequest
    {
        public Guid ProductVariantAttributeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; } = null!;
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditProductVariantAttributeValueRequest : AddProductVariantAttributeValueRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductVariantAttributeValueRequest : ListBoxRequest
    {
        [FromQuery(Name = "$productVariantAttributeId")]
        public string? ProductVariantAttributeId { get; set; }
    }
    public class PagingProductVariantAttributeValueRequest : PagingRequest
    {
        [FromQuery(Name = "$productVariantAttributeId")]
        public string? ProductVariantAttributeId { get; set; }
    }
}
