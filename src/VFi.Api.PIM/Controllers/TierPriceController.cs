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
    public class TierPriceController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<TierPriceController> _logger;
        public TierPriceController(IMediatorHandler mediator, IContextUser context, ILogger<TierPriceController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingTierPriceRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            TierPricePagingQuery query = new TierPricePagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddTierPriceRequest request)
        {
            var productAddCommand = new TierPriceAddCommand(
                Guid.NewGuid(),
                request.StoreId,
                request.ProductId,
                request.StartDate,
                request.EndDate,
                request.Price,
                request.CalculationMethod,
                request.Quantity,
                _context.GetUserId(),
                DateTime.UtcNow
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditTierPriceRequest request)
        {
            var productEditCommand = new TierPriceEditCommand(
                request.Id,
                request.StoreId,
                request.ProductId,
                request.StartDate,
                request.EndDate,
                request.Price,
                request.CalculationMethod,
                request.Quantity,
                _context.GetUserId(),
                DateTime.UtcNow
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new TierPriceDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
