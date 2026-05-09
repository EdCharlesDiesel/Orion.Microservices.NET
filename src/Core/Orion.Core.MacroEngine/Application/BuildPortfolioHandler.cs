using MediatR;
using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Application
{


    public class BuildPortfolioHandler
        : IRequestHandler<BuildPortfolioCommand, List<PortfolioPosition>>
    {
        private readonly IMediator _mediator;
        private readonly IVolatilityService _volatility;

        public BuildPortfolioHandler(IMediator mediator, IVolatilityService volatility)
        {
            _mediator = mediator;
            _volatility = volatility;
        }

        public async Task<List<PortfolioPosition>> Handle(
            BuildPortfolioCommand request,
            CancellationToken ct)
        {
            var signals = await _mediator.Send(new GenerateFxSignalsCommand());

            // Step 1: Filter strongest signals
            var filtered = signals
                .Where(x => x.SignalStrength > 0.5)
                .OrderByDescending(x => x.SignalStrength)
                .Take(10)
                .ToList();

            // Step 2: Enforce currency exposure limits
            var selected = EnforceExposureLimits(filtered, maxPerCurrency: 2);

            // Step 3: Get volatility (ATR or similar)
            var positions = new List<PortfolioPosition>();

            foreach (var s in selected)
            {
                var vol = await _volatility.GetVolatilityAsync(s.Pair);

                positions.Add(new PortfolioPosition
                {
                    Pair = s.Pair,
                    BaseCurrency = s.BaseCurrency,
                    QuoteCurrency = s.QuoteCurrency,
                    Direction = s.Direction,
                    SignalStrength = s.SignalStrength,
                    Confidence = s.Confidence,
                    Volatility = vol
                });
            }

            // Step 4: Risk-based weighting (inverse volatility)
            var totalInvVol = positions.Sum(p => 1.0 / (p.Volatility + 1e-6));

            foreach (var p in positions)
            {
                p.Weight = (1.0 / (p.Volatility + 1e-6)) / totalInvVol;
            }

            // Step 5: Convert to position size
            foreach (var p in positions)
            {
                p.PositionSize = p.Weight * request.Capital;
            }

            return positions;
        }

        // ================= HELPERS =================

        private List<FxSignal> EnforceExposureLimits(
            List<FxSignal> signals,
            int maxPerCurrency)
        {
            var result = new List<FxSignal>();
            var exposure = new Dictionary<string, int>();

            foreach (var s in signals)
            {
                if (!CanAdd(s.BaseCurrency, exposure, maxPerCurrency)) continue;
                if (!CanAdd(s.QuoteCurrency, exposure, maxPerCurrency)) continue;

                result.Add(s);

                Increment(s.BaseCurrency, exposure);
                Increment(s.QuoteCurrency, exposure);
            }

            return result;
        }

        private bool CanAdd(string currency, Dictionary<string, int> exposure, int max)
        {
            return !exposure.ContainsKey(currency) || exposure[currency] < max;
        }

        private void Increment(string currency, Dictionary<string, int> exposure)
        {
            if (!exposure.ContainsKey(currency))
                exposure[currency] = 0;

            exposure[currency]++;
        }
    }
}
