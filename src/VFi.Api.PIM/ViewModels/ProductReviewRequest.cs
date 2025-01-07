using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductReviewRequest
    {
        public string ProductId { get; set; } = null!;
        public string? Title { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public int HelpfulYesTotal { get; set; }
        public int HelpfulNoTotal { get; set; }
        public bool? IsVerifiedPurchase { get; set; }
    }
    public class EditProductReviewRequest : AddProductReviewRequest
    {
        public string Id { get; set; } = null!;
    }

    public class ListBoxProductReviewRequest : ListBoxRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }

        public ProductReviewQueryParams ToBaseQuery() => new ProductReviewQueryParams
        {
            ProductId = ProductId
        };
    }
    public class PagingProductReviewRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }

        public ProductReviewQueryParams ToBaseQuery() => new ProductReviewQueryParams
        {
            ProductId = ProductId
        };
    }

}
