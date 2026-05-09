using Orion.Core.MacroEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Interfaces
{
    public interface ITradingEconomicsClient
    {
        Task<IEnumerable<EconomicIndicator>> GetIndicatorsAsync(string country);
    }
}
