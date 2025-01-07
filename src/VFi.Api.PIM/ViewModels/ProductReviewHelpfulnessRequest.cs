using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductReviewHelpfulnessRequest
    {
        public string ProductReviewId { get; set; } = null!;
        public bool WasHelpful { get; set; }
    }
    public class EditProductReviewHelpfulnessRequest : AddProductReviewHelpfulnessRequest
    {
        public string Id { get; set; }
    }

    public class PagingProductReviewHelpfulnessRequest : PagingRequest
    {
        [FromQuery(Name = "$productReviewId")]
        public string? ProductReviewId { get; set; }
        public ProductReviewHelpfulnessQueryParams ToBaseQuery() => new ProductReviewHelpfulnessQueryParams
        {
            ProductReviewId = ProductReviewId,
        };
    }

}
