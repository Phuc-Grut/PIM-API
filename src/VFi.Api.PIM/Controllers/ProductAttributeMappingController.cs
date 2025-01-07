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
using System.Drawing;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeMappingController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductAttributeMappingController> _logger;
        public ProductAttributeMappingController(IMediatorHandler mediator, IContextUser context, ILogger<ProductAttributeMappingController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductAttributeMappingRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductAttributeMappingPagingQuery query = new ProductAttributeMappingPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductAttributeMappingRequest request)
        {
           
            var productSpecificationAttributeMappingAddCommand = new ProductAttributeMappingAddCommand(
                Guid.NewGuid(),
                request.ProductId,
                request.ProductAttributeId,
                request.TextPrompt,
                request.CustomData,
                request.IsRequired,
                request.AttributeControlTypeId,
                request.DisplayOrder,
                request.ListDetail
          );
            var result = await _mediator.SendCommand(productSpecificationAttributeMappingAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductAttributeMappingRequest request)
        {
            var productSpecificationAttributeMappingEditCommand = new ProductAttributeMappingEditCommand(
                request.Id,
                request.ProductId,
                request.ProductAttributeId,
                request.TextPrompt,
                request.CustomData,
                request.IsRequired,
                request.AttributeControlTypeId,
                request.DisplayOrder,
                request.ListDetail
           );

            var result = await _mediator.SendCommand(productSpecificationAttributeMappingEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductAttributeMappingDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
