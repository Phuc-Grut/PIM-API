using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductCategoryMappingRequest
    {
        public string CategoryId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
    public class EditProductCategoryMappingRequest : AddProductCategoryMappingRequest
    {
        public string Id { get; set; } = null!;
    }

    public class PagingProductCategoryMappingRequest : PagingRequest
    {
        [FromQuery(Name = "$categoryId")]
        public string? CategoryId { get; set; }
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductCategoryMappingQueryParams ToBaseQuery() => new ProductCategoryMappingQueryParams
        {
            CategoryId = CategoryId,
            ProductId = ProductId
        };
    }
 
}
