using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orion.API.TradingEconomics.Entities;

namespace Orion.Core.MacroEngine.Application
{
    public record CalculateCurrencyFactorsCommand : IRequest<List<CurrencyFactorScore>>;
}
