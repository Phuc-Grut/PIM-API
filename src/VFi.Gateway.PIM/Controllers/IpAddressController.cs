using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BE.GATEWAY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpAddressController : ControllerBase
    {
        private IHttpContextAccessor _context;
        private readonly ILogger<IpAddressController> _logger;
        public IpAddressController(ILogger<IpAddressController> logger, IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _logger = logger;
        }
 

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ip = "";
            if (_context.HttpContext.Request.Headers.ContainsKey("cf-connecting-ip"))
                ip = _context?.HttpContext.Request.Headers["cf-connecting-ip"];
            else if (_context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                ip = _context?.HttpContext.Request.Headers["X-Forwarded-For"];
            else
                ip =  _context?.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();


            //cf-connecting-ip
            _logger.LogInformation("---------------");
            foreach (var x in _context?.HttpContext.Request.Headers)
            {
                _logger.LogInformation(x.Key + ":" + x.Value + "  |  ");
            }
            _logger.LogInformation("---------------");
            return Ok(ip);
        }

    }
}
