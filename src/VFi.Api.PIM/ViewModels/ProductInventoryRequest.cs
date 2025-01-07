using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductInventoryRequest
    {
        public string WarehouseId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int? StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public int? PlannedQuantity { get; set; }
    }
    public class EditProductInventoryRequest : AddProductInventoryRequest
    {
        public string Id { get; set; } = null!;
    }
    public class AddProductInventoryRequestList
    {
        public string ProductId { get; set; } = null!;
        public List<AddProductInventoryRequest>? ListInventory { get; set; }
    }

    public class PagingProductInventoryRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        [FromQuery(Name = "$warehouseId")]
        public string? WarehouseId { get; set; }
        public ProductInventoryQueryParams ToBaseQuery() => new ProductInventoryQueryParams
        {
            ProductId = ProductId
        };
    }

}
