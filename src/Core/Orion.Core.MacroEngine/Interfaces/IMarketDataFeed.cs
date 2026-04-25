using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Interfaces
{
    public interface IMarketDataFeed
    {
        Task<MarketTick> GetLatestTickAsync(string pair);
    }
    public interface IExecutionCostModel
    {
        double EstimateSlippage(string pair, double size);
        double EstimateSpread(string pair, double bid, double ask);
    }
}
