using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddTierPriceRequest
    {
        public Guid ProductId { get; set; }
        public Guid? StoreId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CalculationMethod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class EditTierPriceRequest : AddTierPriceRequest
    {
        public Guid Id { get; set; }
    }

    public class PagingTierPriceRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        [FromQuery(Name = "$storeId")]
        public string? StoreId { get; set; }
        [FromQuery(Name = "$startDate")]
        public DateTime? StartDate { get; set; }
        [FromQuery(Name = "$endDate")]
        public DateTime? EndDate { get; set; }

        public TierPriceQueryParams ToBaseQuery() => new TierPriceQueryParams
        {
            ProductId = ProductId,
            StoreId = StoreId,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }

}
