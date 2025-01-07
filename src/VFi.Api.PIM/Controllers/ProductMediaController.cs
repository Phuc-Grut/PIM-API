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
    public class ProductMediaController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductMediaController> _logger;
        public ProductMediaController(IMediatorHandler mediator, IContextUser context, ILogger<ProductMediaController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductMediaRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductMediaPagingQuery query = new ProductMediaPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductMediaRequest request)
        {
            var productAddCommand = new ProductMediaAddCommand(
                Guid.NewGuid(),
                request.ProductId,
                request.Name,
                request.Path,
                request.MediaType,
                request.DisplayOrder,
                _context.GetUserId(),
                DateTime.UtcNow
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPost("add-list")]
        public async Task<IActionResult> AddList([FromBody] AddProductMediaRequestList request)
        {
            var create = _context.GetUserId();
            var createdDate = DateTime.Now;
            var listAtt = request.ListAtt?.Select(x => new ProductMediaDto()
            {
                Id = Guid.NewGuid(),
                ProductId = x.ProductId,
                Name = x.Name,
                Path = x.Path,
                MediaType = x.MediaType,
                DisplayOrder = x.DisplayOrder,
                CreatedDate = createdDate,
                CreatedBy = create,
            }).ToList();
            var ProductMediaAddListCommand = new ProductMediaAddListCommand(
       listAtt
      );
            var result = await _mediator.SendCommand(ProductMediaAddListCommand);
            return Ok(result);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductMediaRequest request)
        {
            var productEditCommand = new ProductMediaEditCommand(
                new Guid(request.Id),
                request.ProductId,    
                request.Name,
                request.Path,
                request.MediaType,
                request.DisplayOrder,
                _context.GetUserId(),
                DateTime.UtcNow
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductMediaDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
