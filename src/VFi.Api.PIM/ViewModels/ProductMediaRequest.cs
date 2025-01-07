using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductMediaRequest
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string Path { get; set; } = null!;
        public string MediaType { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }
    public class EditProductMediaRequest : AddProductMediaRequest
    {
        public string Id { get; set; } = null!;
    }
    public class AddProductMediaRequestList
    {
        public List<AddProductMediaRequest>? ListAtt { get; set; }
    }
    public class PagingProductMediaRequest : PagingRequest
    {
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }

        public ProductMediaQueryParams ToBaseQuery() => new ProductMediaQueryParams
        {
            ProductId = ProductId

        };
    }

}
