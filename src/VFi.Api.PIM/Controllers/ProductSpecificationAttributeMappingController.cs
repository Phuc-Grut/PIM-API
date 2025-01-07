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
    public class ProductSpecificationAttributeMappingController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductSpecificationAttributeMappingController> _logger;
        public ProductSpecificationAttributeMappingController(IMediatorHandler mediator, IContextUser context, ILogger<ProductSpecificationAttributeMappingController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductSpecificationAttributeMappingRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductSpecificationAttributeMappingPagingQuery query = new ProductSpecificationAttributeMappingPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductSpecificationAttributeMappingRequest request)
        {
            var productSpecificationAttributeMappingAddCommand = new ProductSpecificationAttributeMappingAddCommand(
                Guid.NewGuid(),
                new Guid(request.ProductId),
                new Guid(request.SpecificationAttributeId),
                new Guid(request.SpecificationAttributeOptionId),
                request.DisplayOrder
          );
            var result = await _mediator.SendCommand(productSpecificationAttributeMappingAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductSpecificationAttributeMappingRequest request)
        {
            var productSpecificationAttributeMappingEditCommand = new ProductSpecificationAttributeMappingEditCommand(
                Guid.NewGuid(),
                 new Guid(request.ProductId),
                new Guid(request.SpecificationAttributeId),
                new Guid(request.SpecificationAttributeOptionId),
                request.DisplayOrder
           );

            var result = await _mediator.SendCommand(productSpecificationAttributeMappingEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductSpecificationAttributeMappingDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
