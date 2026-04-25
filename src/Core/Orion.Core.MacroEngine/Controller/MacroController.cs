using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orion.Core.MacroEngine.Application;
using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Controller
{
    [ApiController]
    [Route("api/macro")]
    public class MacroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MacroController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ingest/{country}")]
        public async Task<IActionResult> Ingest(string country)
        {
            await _mediator.Send(new IngestMacroDataCommand(country));
            return Ok();
        }

        [HttpPost("normalize")]
        public async Task<IActionResult> Normalize()
        {
            await _mediator.Send(new NormalizeMacroDataCommand());
            return Ok();
        }

        [HttpGet("factors")]
        public async Task<IActionResult> GetFactors()
        {
            var result = await _mediator.Send(new CalculateCurrencyFactorsCommand());
            return Ok(result);
        }

        [HttpGet("signals")]
        public async Task<IActionResult> GetSignals()
        {
            var result = await _mediator.Send(new GenerateFxSignalsCommand());
            return Ok(result);
        }

        [HttpGet("portfolio")]
        public async Task<IActionResult> GetPortfolio(double capital = 100000)
        {
            var result = await _mediator.Send(new BuildPortfolioCommand(capital));
            return Ok(result);
        }
    }
}
