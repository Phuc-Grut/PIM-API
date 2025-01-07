using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductPackageRequest
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
    }
    public class EditProductPackageRequest : AddProductPackageRequest
    {
        public Guid Id { get; set; }
    }

    public class PagingProductPackageRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductPackageQueryParams ToBaseQuery() => new ProductPackageQueryParams
        {
            ProductId = ProductId,
        };
    }

}
