using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpiderController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<SpiderController> _logger;
        public SpiderController(IMediatorHandler mediator, IContextUser context, ILogger<SpiderController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        /// <remarks>
        /// Sample request:
        ///
        ///    https://api.ipify.org/?format=json
        ///
        /// </remarks>
        [HttpGet("get-by-url")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var query = new SpiderQuery(url);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-mercari-item")]
        public async Task<IActionResult> GetMercariItem(string itemId)
        {

            var query = new MercariItemQuery(itemId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-mercari-home")]
        public async Task<IActionResult> GetMercariHome(int take = 60)
        {

            var query = new MercariHomeItemQuery(take);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-mercari-search")]
        public async Task<IActionResult> GetMercariSearch(int? categoryId,string? keyword, int pageIndex=1)
        {

            var query = new MercariSearchQuery("", categoryId);
            query.PageIndex= pageIndex; query.Keyword = String.IsNullOrEmpty(keyword)?"":keyword;
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("get-auction-item")]
        public async Task<IActionResult> GetAuctionItem(string itemId)
        {

            var query = new AuctionItemQuery(itemId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-auction-related-items")]
        public async Task<IActionResult> GetAuctionRelatedItems(string itemId)
        {

            var query = new AuctionRelatedItemsQuery(itemId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
