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
    public class ProductSpecificationCodeController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductSpecificationCodeController> _logger;
        public ProductSpecificationCodeController(IMediatorHandler mediator, IContextUser context, ILogger<ProductSpecificationCodeController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductSpecificationCodeRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductSpecificationCodePagingQuery query = new ProductSpecificationCodePagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("paging-to")]
        public async Task<IActionResult> PagingTo([FromQuery] PagingProductSpecificationCodeRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductSpecificationCodePagingQueryTo query = new ProductSpecificationCodePagingQueryTo(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductSpecificationCodeRequest request)
        {
            var productSpecificationCodeAddCommand = new ProductSpecificationCodeAddCommand(
                Guid.NewGuid(),
                new Guid(request.ProductId),
                request.Name,
                request.DuplicateAllowed,
                request.Status,
                request.DataTypes,
                request.DisplayOrder
          );
            var result = await _mediator.SendCommand(productSpecificationCodeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductSpecificationCodeRequest request)
        {
            var productSpecificationCodeEditCommand = new ProductSpecificationCodeEditCommand(
                Guid.NewGuid(),
                 new Guid(request.ProductId),
                request.Name,
                request.DuplicateAllowed,
                request.Status,
                request.DataTypes,
                request.DisplayOrder
           );

            var result = await _mediator.SendCommand(productSpecificationCodeEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductSpecificationCodeDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
