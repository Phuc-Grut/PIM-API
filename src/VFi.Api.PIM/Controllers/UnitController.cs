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
    public class UnitController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<UnitController> _logger;
        public UnitController(IMediatorHandler mediator, IContextUser context, ILogger<UnitController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxUnitRequest request)
        {
            var result = await _mediator.Send(new UnitQueryListBox(request.Status, request.GroupUnitId, request.Keyword, request.NullAble ?? false));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new UnitPagingQuery(request.Keyword ?? "", request.Filter ?? "", request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddUnitRequest request)
        {
            var unitAddCommand = new UnitAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.NamePlural,
              request.Description, request.GroupUnitId,
              request.Rate,
              request.IsDefault,
              request.DisplayLocale,
              request.Status,
              request.DisplayOrder,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(unitAddCommand);
            return Ok(result);
        }
        public class UnitEdit
        {
            public UnitEdit()
            {
            }

            public Guid Id { get; set; }
            public string? Code { get; set; }
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditUnitRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var unitEditCommand = new UnitEditCommand(
               request.Id,
               request.Code,
               request.Name,
               request.NamePlural,
               request.Description,
               request.GroupUnitId,
               request.Rate,
               request.IsDefault,
               request.DisplayLocale,
               request.Status,
               request.DisplayOrder,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(unitEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new UnitSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
                var result = await _mediator.SendCommand(new UnitDeleteCommand(new Guid(id)));
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(new ValidationResult("Không thể xóa khoản mục đã sử dụng để tạo dữ liệu khác!"));
            }
        }
    }
}
