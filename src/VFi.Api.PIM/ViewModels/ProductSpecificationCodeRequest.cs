using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductSpecificationCodeRequest
    {
        public string ProductId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool DuplicateAllowed { get; set; }
        public int Status { get; set; }
        public int DataTypes { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditProductSpecificationCodeRequest : AddProductSpecificationCodeRequest
    {
        public string Id { get; set; } = null!;
    }

    public class PagingProductSpecificationCodeRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductSpecificationCodeQueryParams ToBaseQuery() => new ProductSpecificationCodeQueryParams
        {
            ProductId = ProductId
        };
    }

}
