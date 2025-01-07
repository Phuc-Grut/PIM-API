using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductManufacturerMappingRequest
    {
        public string ManufacturerId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
    public class EditProductManufacturerMappingRequest : AddProductManufacturerMappingRequest
    {
        public string Id { get; set; } = null!;
    }
    public class PagingProductManufacturerMappingRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        [FromQuery(Name = "$manufacturerId")]
        public string? ManufacturerId { get; set; }

        public ProductManufacturerMappingQueryParams ToBaseQuery() => new ProductManufacturerMappingQueryParams
        {
            ProductId = ProductId,
            ManufacturerId = ManufacturerId
        };
    }

}
