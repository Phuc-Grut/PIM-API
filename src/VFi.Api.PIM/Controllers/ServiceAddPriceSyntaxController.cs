using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAddPriceSyntaxController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ServiceAddPriceSyntaxController> _logger;

        public ServiceAddPriceSyntaxController(IMediatorHandler mediator, IContextUser context, ILogger<ServiceAddPriceSyntaxController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new ServiceAddPriceSyntaxComboboxQuery());

            return Ok(result);
        }
    }
}
