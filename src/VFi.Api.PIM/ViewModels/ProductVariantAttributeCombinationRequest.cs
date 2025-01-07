using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductVariantAttributeCombinationRequest
    {
        public string? Name { get; set; }
        public Guid ProductId { get; set; }
        public string? Sku { get; set; }
        public string? Gtin { get; set; }
        public string? ManufacturerPartNumber { get; set; }
        public decimal? Price { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? BasePriceAmount { get; set; }
        public int? BasePriceBaseAmount { get; set; }
        public string? AssignedMediaFileIds { get; set; }
        public bool IsActive { get; set; }
        public Guid? DeliveryTimeId { get; set; }
        public Guid? QuantityUnitId { get; set; }
        public string? AttributesXml { get; set; }
        public int StockQuantity { get; set; }
        public bool AllowOutOfStockOrders { get; set; }
    }
    public class EditProductVariantAttributeCombinationRequest : AddProductVariantAttributeCombinationRequest
    {
        public string Id { get; set; } = null!;
    }
    public class ListBoxProductVariantAttributeCombinationRequest : ListBoxRequest
    {
        [FromQuery(Name = "$deliveryTimeId")]
        public string? DeliveryTimeId { get; set; }
        [FromQuery(Name = "$quantityUnitId")]
        public string? QuantityUnitId { get; set; }
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductVariantAttributeCombinationQueryParams ToBaseQuery() => new ProductVariantAttributeCombinationQueryParams
        {
            QuantityUnitId = QuantityUnitId,
            DeliveryTimeId = DeliveryTimeId,
            ProductId = ProductId
        };
    }
    public class PagingProductVariantAttributeCombinationRequest : PagingRequest
    {
        [FromQuery(Name = "$deliveryTimeId")]
        public string? DeliveryTimeId { get; set; }
        [FromQuery(Name = "$quantityUnitId")]
        public string? QuantityUnitId { get; set; }
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductVariantAttributeCombinationQueryParams ToBaseQuery() => new ProductVariantAttributeCombinationQueryParams
        {
            QuantityUnitId = QuantityUnitId,
            DeliveryTimeId = DeliveryTimeId,
            ProductId = ProductId
        };
    }

}
