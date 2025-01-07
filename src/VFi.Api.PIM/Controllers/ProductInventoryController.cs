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
    public class ProductInventoryController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductInventoryController> _logger;
        public ProductInventoryController(IMediatorHandler mediator, IContextUser context, ILogger<ProductInventoryController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductInventoryRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductInventoryPagingQuery query = new ProductInventoryPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductInventoryRequest request)
        {
            var productAddCommand = new ProductInventoryAddCommand(
                Guid.NewGuid(),
                new Guid(request.WarehouseId),
                new Guid(request.ProductId),
                request.StockQuantity,
                request.PlannedQuantity,
                request.ReservedQuantity,
                _context.GetUserId(),
                DateTime.UtcNow
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }
        [HttpPost("add-list")]
        public async Task<IActionResult> AddList([FromBody] AddProductInventoryRequestList request)
        {
            var create = _context.GetUserId();
            var createdDate = DateTime.Now;
            var listAtt = request.ListInventory?.Select(x => new ProductInventoryDto()
            {
                Id = Guid.NewGuid(),
                WarehouseId = new Guid(x.WarehouseId),
                ProductId=new Guid(x.ProductId),
                StockQuantity = x.StockQuantity,
                PlannedQuantity =x.PlannedQuantity,
                ReservedQuantity=x.ReservedQuantity,
                CreatedBy = create,
                CreatedDate = createdDate
            }).ToList();
            var ProductInventoryAddListCommand = new ProductInventoryAddListCommand(
        new Guid(request.ProductId),
       listAtt
      );
            var result = await _mediator.SendCommand(ProductInventoryAddListCommand);
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductInventoryRequest request)
        {
            var productEditCommand = new ProductInventoryEditCommand(
                new Guid(request.Id),
                new Guid(request.WarehouseId),
                new Guid(request.ProductId),
                request.StockQuantity,
                request.PlannedQuantity,
                request.ReservedQuantity,
                _context.GetUserId(),
                DateTime.UtcNow
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductInventoryDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
