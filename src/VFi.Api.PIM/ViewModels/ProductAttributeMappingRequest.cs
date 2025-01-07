using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductAttributeMappingRequest
    {
        public Guid ProductAttributeId { get; set; }
        public Guid ProductId { get; set; }
        public string? TextPrompt { get; set; }
        public string? CustomData { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public List<AddProductVariantAttributeValueDto>? ListDetail { get; set; }
    }
    public class EditProductAttributeMappingRequest : AddProductAttributeMappingRequest
    {
        public Guid Id { get; set; }
    }
    public class PagingProductAttributeMappingRequest : PagingRequest
    {
        [FromQuery(Name = "$productAttributeId")]
        public string? ProductAttributeId { get; set; }
        [FromQuery(Name = "$productId")]
        public string? ProductId { get; set; }
        public ProductAttributeMappingQueryParams ToBaseQuery() => new ProductAttributeMappingQueryParams
        {
            ProductAttributeId = ProductAttributeId,
            ProductId = ProductId
        };
    }

}
