using Consul;
using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryMappingController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductCategoryMappingController> _logger;
        public ProductCategoryMappingController(IMediatorHandler mediator, IContextUser context, ILogger<ProductCategoryMappingController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
      
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductCategoryMappingRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductCategoryMappingPagingQuery query = new ProductCategoryMappingPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductCategoryMappingRequest request)
        {
            var productAddCommand = new ProductCategoryMappingAddCommand(
                Guid.NewGuid(),
                new Guid(request.CategoryId),
                new Guid(request.ProductId),
                request.DisplayOrder
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductCategoryMappingRequest request)
        {
            var productEditCommand = new ProductCategoryMappingEditCommand(
                Guid.NewGuid(),
                new Guid(request.CategoryId),
                new Guid(request.ProductId),
                request.DisplayOrder
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductCategoryMappingDeleteCommand(new Guid(id)));

            return Ok(result);
        }
       
    }
}
