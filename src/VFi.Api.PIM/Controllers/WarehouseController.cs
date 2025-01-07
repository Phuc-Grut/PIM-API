using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<WarehouseController> _logger;
        public WarehouseController(IMediatorHandler mediator, IContextUser context, ILogger<WarehouseController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new WarehouseQueryListBox(request.Keyword, request.Status));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new WarehousePagingQuery(request.Keyword ?? "", request.Filter ?? "", request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddWarehouseRequest request)
        {
            var warehouseAddCommand = new WarehouseAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.Latitude,
              request.Longitude,
              request.Company,
              request.Country,
              request.Province,
              request.District,
              request.Ward,
              request.Address,
              request.PostalCode,
              request.PhoneNumber,
              request.Api,
              request.Token,
              request.DisplayOrder,
              request.Status,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(warehouseAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditWarehouseRequest request)
        {
            var warehouseEditCommand = new WarehouseEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name,
               request.Latitude,
               request.Longitude,
               request.Company,
               request.Country,
               request.Province,
               request.District,
               request.Ward,
               request.Address,
               request.PostalCode,
               request.PhoneNumber,
               request.Api,
               request.Token,
               request.DisplayOrder,
               request.Status,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );
            var result = await _mediator.SendCommand(warehouseEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var warehouseSortCommand = new WarehouseSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(warehouseSortCommand);
                return Ok(result);
            }
            else
            {
                return BadRequest("Please input list sort");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new WarehouseDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
