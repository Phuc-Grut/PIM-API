using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddRelatedProductRequest
    {
        public Guid ProductId1 { get; set; }
        public Guid ProductId2 { get; set; }
        public int DisplayOrder { get; set; }
     
    }
    public class EditRelatedProductRequest : AddRelatedProductRequest
    {
        public Guid Id { get; set; }
    }
    public class AddListRelatedProductRequest
    {
        public Guid ProductId1 { get; set; }
        public List<Guid>? ListProductId2 { get; set; }

    }

    public class PagingRelatedProductRequest : PagingRequest
    {
        [FromQuery(Name = "$productId1")]
        public string? ProductId1 { get; set; }

        [FromQuery(Name = "$productId2")]
        public string? ProductId2 { get; set; }
        public RelatedProductQueryParams ToBaseQuery() => new RelatedProductQueryParams
        {
            ProductId1 = ProductId1,
            ProductId2 = ProductId2,
        
        };
    }

}
