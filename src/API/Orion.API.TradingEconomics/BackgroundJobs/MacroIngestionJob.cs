using MediatR;
using Orion.API.TradingEconomics.Commands;
using Quartz;

namespace Orion.Core.MacroEngine.BackgroundJobs
{
    public class MacroIngestionJob : IJob
    {
        private readonly IMediator _mediator;

        public MacroIngestionJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var countries = new[]
            {
            "United States",
            "Euro Area",
            "Japan",
            "United Kingdom",
            "South Africa"
        };

            foreach (var country in countries)
            {
                var count = await _mediator.Send(new IngestMacroDataCommand(country));

                Console.WriteLine($"[{country}] Ingested: {count} records");
            }
        }
    }
}
