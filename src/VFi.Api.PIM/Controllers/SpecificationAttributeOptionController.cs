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
    public class SpecificationAttributeOptionController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<SpecificationAttributeOptionController> _logger;
        public SpecificationAttributeOptionController(IMediatorHandler mediator, IContextUser context, ILogger<SpecificationAttributeOptionController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new SpecificationAttributeOptionQueryAll());
            return Ok(result);
        }

        //[HttpGet("get-listbox")]
        //public async Task<IActionResult> Get([FromQuery] ListBoxSpecificationAttributeOptionRequest request)
        //{
        //    var result = await _mediator.Send(new SpecificationAttributeOptionQueryListBox(request.SpecificationAttributeId, request.Keyword));
        //    return Ok(result);
        //}

        //[HttpGet("paging")]
        //public async Task<IActionResult> Paging([FromQuery] PagingSpecificationAttributeOptionRequest request)
        //{
        //    var pageSize = request.Top;
        //    var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        //    SpecificationAttributeOptionPagingQuery query = new SpecificationAttributeOptionPagingQuery(request.Keyword, request.SpecificationAttributeId, pageSize, pageIndex);

        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        //[HttpPost("add")]
        //public async Task<IActionResult> Add([FromBody] AddSpecificationAttributeOptionRequest request)
        //{
        //    var specificationAttributeOptionAddCommand = new SpecificationAttributeOptionAddCommand(
        //      Guid.NewGuid(),
        //      new Guid(request.SpecificationAttributeId),
        //      request.Name,
        //      request.Alias,
        //      request.DisplayOrder,
        //      request.NumberValue,
        //      request.MediaFileId,
        //      request.Color,
        //      _context.GetUserId(),
        //      DateTime.Now
        //  );
        //    var result = await _mediator.SendCommand(specificationAttributeOptionAddCommand);
        //    return Ok(result);
        //}

        //[HttpPut("edit")]
        //public async Task<IActionResult> Put([FromBody] EditSpecificationAttributeOptionRequest request)
        //{
        //    var specificationAttributeOptionEditCommand = new SpecificationAttributeOptionEditCommand(
        //       new Guid(request.Id),
        //       new Guid(request.SpecificationAttributeId),
        //       request.Name,
        //       request.Alias,
        //       request.DisplayOrder,
        //       request.NumberValue,
        //       request.MediaFileId,
        //       request.Color,
        //       _context.GetUserId(),
        //       DateTime.Now
        //   );

        //    var result = await _mediator.SendCommand(specificationAttributeOptionEditCommand);

        //    return Ok(result);
        //}

        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var result = await _mediator.SendCommand(new SpecificationAttributeOptionDeleteCommand(new Guid(id)));

        //    return Ok(result);
        //}
    }
}
