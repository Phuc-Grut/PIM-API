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
    public class ProductReviewHelpfulnessController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductReviewHelpfulnessController> _logger;
        public ProductReviewHelpfulnessController(IMediatorHandler mediator, IContextUser context, ILogger<ProductReviewHelpfulnessController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductReviewHelpfulnessRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductReviewHelpfulnessPagingQuery query = new ProductReviewHelpfulnessPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductReviewHelpfulnessRequest request)
        {
            var productAddCommand = new ProductReviewHelpfulnessAddCommand(
                Guid.NewGuid(),
                new Guid(request.ProductReviewId),
                request.WasHelpful
          );
            var result = await _mediator.SendCommand(productAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductReviewHelpfulnessRequest request)
        {
            var productEditCommand = new ProductReviewHelpfulnessEditCommand(
                new Guid(request.Id),
                new Guid(request.ProductReviewId),
                request.WasHelpful
           );

            var result = await _mediator.SendCommand(productEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductReviewHelpfulnessDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
