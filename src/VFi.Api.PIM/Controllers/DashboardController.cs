    using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Interfaces;
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
    public class DashboardController : ControllerBase
    {
        private readonly IPIMContextProcedures _pimContextProcedures;
        public DashboardController(IPIMContextProcedures pimContextProcedures)
        {
            _pimContextProcedures = pimContextProcedures;
        }

        [HttpGet("count-products")]
        public async Task<IActionResult> Get()
        {
            var result = await _pimContextProcedures.SP_COUNT_PRODUCTSAsync();
            return Ok(result);
        }
        [HttpGet("count-products-by-type")]
        public async Task<IActionResult> Count_Product_By_Type()
        {
            var result = await _pimContextProcedures.SP_COUNT_PRODUCT_BY_PRODUCTTYPEAsync();
            return Ok(result);
        }
        [HttpGet("get-top-category")]
        public async Task<IActionResult> Get_Top_Category()
        {
            var result = await _pimContextProcedures.SP_GET_TOP_CATEGORYAsync();
            return Ok(result);
        }
        [HttpGet("get-top-manufacture")]
        public async Task<IActionResult> Get_Top_Manufacture()
        {
            var result = await _pimContextProcedures.SP_GET_TOP_MANUFACTURERAsync();
            return Ok(result);
        }
        [HttpGet("get-top-product-inventory")]
        public async Task<IActionResult> Get_Top_Product_Inventory()
        {
            var result = await _pimContextProcedures.SP_GET_TOP_PRODUCTS_INVENTORYAsync();
            return Ok(result);
        }
        [HttpGet("get-top-brand")]
        public async Task<IActionResult> Get_Top_Brand()
        {
            var result = await _pimContextProcedures.SP_GET_TOP_PRODUCT_BRANDAsync();
            return Ok(result);
        }
        [HttpGet("get-top-new-product")]
        public async Task<IActionResult> Get_Top_New_Product()
        {
            var result = await _pimContextProcedures.SP_GET_TOP_NEW_PRODUCTAsync();
            return Ok(result);
        }
    }
}
