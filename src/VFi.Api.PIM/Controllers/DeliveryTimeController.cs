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
    public class DeliveryTimeController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<DeliveryTimeController> _logger;
        public DeliveryTimeController(IMediatorHandler mediator, IContextUser context, ILogger<DeliveryTimeController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new DeliveryTimeQueryListBox(request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new DeliveryTimePagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddDeliveryTimeRequest request)
        {
            var deliveryTimeAddCommand = new DeliveryTimeAddCommand(
              Guid.NewGuid(),
              request.Name,
              request.IsDefault,
              request.MinDays,
              request.MaxDays,
              request.DisplayOrder,
              request.Status,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(deliveryTimeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditDeliveryTimeRequest request)
        {
            var deliveryTimeEditCommand = new DeliveryTimeEditCommand(
               new Guid(request.Id),
               request.Name,
               request.IsDefault,
               request.MinDays,
               request.MaxDays,
               request.DisplayOrder,
              request.Status,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(deliveryTimeEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new DeliveryTimeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(sortCommand);
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
            var result = await _mediator.SendCommand(new DeliveryTimeDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
