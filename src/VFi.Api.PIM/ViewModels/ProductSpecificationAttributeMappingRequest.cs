using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductSpecificationAttributeMappingRequest
    {
        public string SpecificationAttributeId { get; set; } = null!;
        public string SpecificationAttributeOptionId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
    public class EditProductSpecificationAttributeMappingRequest : AddProductSpecificationAttributeMappingRequest
    {
        public string Id { get; set; } = null!;
    }

    public class PagingProductSpecificationAttributeMappingRequest : PagingRequest
    {
        [FromQuery(Name = "$specificationAttributeOptionId")]
        public string? SpecificationAttributeOptionId { get; set; }
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductSpecificationAttributeMappingQueryParams ToBaseQuery() => new ProductSpecificationAttributeMappingQueryParams
        {
            SpecificationAttributeOptionId = SpecificationAttributeOptionId,
            ProductId = ProductId
        };
    }

}
