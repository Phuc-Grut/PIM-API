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
    public class ProductServiceAddController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductServiceAddController> _logger;
        public ProductServiceAddController(IMediatorHandler mediator, IContextUser context, ILogger<ProductServiceAddController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductServiceAddRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductServiceAddPagingQuery query = new ProductServiceAddPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductServiceAddRequest request)
        {
            var productAddCommand = new ProductServiceAddAddCommand(
                Guid.NewGuid(),
                request.ProductId,
                request.ServiceAddId,
                request.PayRequired,
                request.Price,
                request.MaxPrice,
                request.CalculationMethod,
                request.PriceSyntax,
                request.MinPrice,
                request.Currency,
                request.Status,
                _context.GetUserId(),
                DateTime.UtcNow
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductServiceAddRequest request)
        {
            var productEditCommand = new ProductServiceAddEditCommand(
                new Guid(request.Id),
                 request.ProductId,
                request.ServiceAddId,
                request.PayRequired,
                request.Price,
                request.MaxPrice,
                request.CalculationMethod,
                request.PriceSyntax,
                request.MinPrice,
                request.Currency,
                request.Status,
                _context.GetUserId(),
                DateTime.UtcNow
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductServiceAddDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
