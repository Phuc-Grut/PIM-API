//using VFi.Api.PIM.ViewModels;
//using VFi.Application.PIM.Commands;
//using VFi.Application.PIM.DTOs;
//using VFi.Application.PIM.Queries;
//using VFi.Domain.PIM.Models;
//using VFi.NetDevPack.Context;
//using VFi.NetDevPack.Mediator;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.ComponentModel.DataAnnotations;

//namespace VFi.Api.PIM.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductAttributeOptionController : ControllerBase
//    {
//        private readonly IContextUser _context;
//        private readonly IMediatorHandler _mediator;
//        private readonly ILogger<ProductAttributeOptionController> _logger;
//        public ProductAttributeOptionController(IMediatorHandler mediator, IContextUser context, ILogger<ProductAttributeOptionController> logger)
//        {
//            _mediator = mediator;
//            _context = context;
//            _logger = logger;
//        }

//        [HttpGet("get-listbox")]
//        public async Task<IActionResult> Get([FromQuery] ListBoxProductAttributeOptionRequest request)
//        {
//            var result = await _mediator.Send(new ProductAttributeOptionQueryListBox(request.ProductAttributeId, request.Keyword));
//            return Ok(result);
//        }

//        [HttpGet("paging")]
//        public async Task<IActionResult> Paging([FromQuery] PagingProductAttributeRequest request)
//        {
//            var pageSize = request.Top;
//            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

//            ProductAttributeOptionPagingQuery query = new ProductAttributeOptionPagingQuery(request.Keyword, request.ProductAttributeId, pageSize, pageIndex);

//            var result = await _mediator.Send(query);
//            return Ok(result);
//        }

//        [HttpPost("add")]
//        public async Task<IActionResult> Add([FromBody] AddProductAttributeOptionRequest request)
//        {
//            var productAttributeOptionAddCommand = new ProductAttributeOptionAddCommand(
//              Guid.NewGuid(),
//              new Guid(request.ProductAttributeId),
//              request.Name,
//              request.Alias,
//              request.MediaFileId,
//              request.Color,
//              request.PriceAdjustment,
//              request.WeightAdjustment,
//              request.IsPreSelected,
//              request.DisplayOrder,
//              request.ValueTypeId,
//              String.IsNullOrEmpty(request.LinkedProductId) ? Guid.Empty : new Guid(request.LinkedProductId),
//              request.Quantity,
//              _context.GetUserId(),
//              DateTime.Now
//          );
//            var result = await _mediator.SendCommand(productAttributeOptionAddCommand);
//            return Ok(result);
//        }

//        [HttpPut("edit")]
//        public async Task<IActionResult> Put([FromBody] EditProductAttributeOptionRequest request)
//        {
//            var productAttributeOptionEditCommand = new ProductAttributeOptionEditCommand(
//               new Guid(request.Id),
//               new Guid(request.ProductAttributeId),
//               request.Name,
//               request.Alias,
//               request.MediaFileId,
//               request.Color,
//               request.PriceAdjustment,
//               request.WeightAdjustment,
//               request.IsPreSelected,
//               request.DisplayOrder,
//               request.ValueTypeId,
//               String.IsNullOrEmpty(request.LinkedProductId) ? Guid.Empty : new Guid(request.LinkedProductId),
//               request.Quantity,
//               _context.GetUserId(),
//               DateTime.Now
//           );

//            var result = await _mediator.SendCommand(productAttributeOptionEditCommand);

//            return Ok(result);
//        }

//        [HttpDelete("delete/{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var result = await _mediator.SendCommand(new ProductAttributeOptionDeleteCommand(new Guid(id)));

//            return Ok(result);
//        }
//    }
//}
