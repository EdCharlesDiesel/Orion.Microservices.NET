using Microsoft.AspNetCore.Mvc;
using Orion.API.TradingEconomics.Engine;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Controllers
{
    [ApiController]
    [Route("api/live-trading")]
    public sealed class LiveTradingController : ControllerBase
    {
        private readonly LiveTradingOrchestrator _orchestrator;

        public LiveTradingController(LiveTradingOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpPost("run")]
        public ActionResult<LiveTradingResult> Run([FromBody] LiveTradingRequest request)
        {
            //var result = _orchestrator.Run(
            //    request.MarketInput,
            //    request.Account,
            //    request.OrderBook);

            //return Ok(result);

            return Ok();
        }
    }
}
