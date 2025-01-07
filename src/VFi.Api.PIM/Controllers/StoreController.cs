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
    public class StoreController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<StoreController> _logger;
        public StoreController(IMediatorHandler mediator, IContextUser context, ILogger<StoreController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new StoreQueryListBox(request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new StorePagingQuery(request.Keyword ?? "", request.Filter ?? "", request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddStoreRequest request)
        {
            var storeAddCommand = new StoreAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.Description,
              request.Address,
              request.Phone,
              request.DisplayOrder,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(storeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditStoreRequest request)
        {
            var storeEditCommand = new StoreEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name,
               request.Description,
               request.Address,
               request.Phone,
               request.DisplayOrder,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(storeEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var storeSortCommand = new StoreSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(storeSortCommand);
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
            var result = await _mediator.SendCommand(new StoreDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
