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
using static MassTransit.ValidationResultExtensions;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupUnitController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<GroupUnitController> _logger;
        public GroupUnitController(IMediatorHandler mediator, IContextUser context, ILogger<GroupUnitController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxGroupUnitRequest request)
        {
            var result = await _mediator.Send(new GroupUnitQueryListBox(request.Status, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new GroupUnitPagingQuery(request.Keyword ?? "", request.Filter ?? "", request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddGroupUnitRequest request)
        {
            var groupUnitAddCommand = new GroupUnitAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.Description,
              request.Status,
              request.DisplayOrder,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(groupUnitAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditGroupUnitRequest request)
        {
            var groupUnitEditCommand = new GroupUnitEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name,
               request.Description,
               request.Status,
               request.DisplayOrder,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(groupUnitEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new GroupUnitSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
            try
            {
                var result = await _mediator.SendCommand(new GroupUnitDeleteCommand(new Guid(id)));
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(new ValidationResult("In use, cannot be deleted"));
            }
        }
    }
}
