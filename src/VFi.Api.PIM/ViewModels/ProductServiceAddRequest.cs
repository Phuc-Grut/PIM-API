using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductServiceAddRequest
    {
        public Guid? ProductId { get; set; }
        public Guid? ServiceAddId { get; set; }
        public int? PayRequired { get; set; }
        public decimal? Price { get; set; }
        public decimal? MaxPrice { get; set; }
        public int CalculationMethod { get; set; }
        public string? PriceSyntax { get; set; }
        public decimal? MinPrice { get; set; }
        public string? Currency { get; set; }
        public int? Status { get; set; }
    }
    public class EditProductServiceAddRequest : AddProductServiceAddRequest
    {
        public string Id { get; set; }
    }

    public class PagingProductServiceAddRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        [FromQuery(Name = "$serviceAddId")]
        public string? ServiceAddId { get; set; }
        [FromQuery(Name = "$startDate")]
        public DateTime? StartDate { get; set; }
        [FromQuery(Name = "$endDate")]
        public DateTime? EndDate { get; set; }

        public ProductServiceAddQueryParams ToBaseQuery() => new ProductServiceAddQueryParams
        {
            ProductId = ProductId,
            ServiceAddId = ServiceAddId,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }

}
