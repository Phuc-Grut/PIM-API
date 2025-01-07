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
    public class ProductPackageController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductPackageController> _logger;
        public ProductPackageController(IMediatorHandler mediator, IContextUser context, ILogger<ProductPackageController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductPackageRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductPackagePagingQuery query = new ProductPackagePagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductPackageRequest request)
        {
            var productAddCommand = new ProductPackageAddCommand(
                Guid.NewGuid(),
                request.ProductId,
                request.Name,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                _context.GetUserId(),
                DateTime.UtcNow
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductPackageRequest request)
        {
            var productEditCommand = new ProductPackageEditCommand(
                request.Id,
               request.ProductId,
                request.Name,
                request.Weight,
                request.Length,
                request.Width,
                request.Height,
                _context.GetUserId(),
                DateTime.UtcNow
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductPackageDeleteCommand(new Guid(id)));

            return Ok(result);
        }

        [HttpGet("get-by-products")]
        public async Task<IActionResult> GetPrice([FromQuery] FilterProductQueryPrice request)
        {
            var rs = await _mediator.Send(new ProductPackageQueryByProducts(request.ListProduct));
            return Ok(rs);
        }
    }
}
