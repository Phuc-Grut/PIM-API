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
    public class ProductReviewController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductReviewController> _logger;
        public ProductReviewController(IMediatorHandler mediator, IContextUser context, ILogger<ProductReviewController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductReviewRequest request)
        {
            var result = await _mediator.Send(new ProductReviewQueryListBox(request.ToBaseQuery(), request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductReviewRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductReviewPagingQuery query = new ProductReviewPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductReviewRequest request)
        {
            var productAddCommand = new ProductReviewAddCommand(
                Guid.NewGuid(),
                new Guid(request.ProductId),
                request.Title,
                request.ReviewText,
                request.Rating,
                request.HelpfulYesTotal,
                request.HelpfulNoTotal,
                request.IsVerifiedPurchase
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductReviewRequest request)
        {
            var productEditCommand = new ProductReviewEditCommand(
                new Guid(request.Id),
                new Guid(request.ProductId),
                request.Title,
                request.ReviewText,
                request.Rating,
                request.HelpfulYesTotal,
                request.HelpfulNoTotal,
                request.IsVerifiedPurchase
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductReviewDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
