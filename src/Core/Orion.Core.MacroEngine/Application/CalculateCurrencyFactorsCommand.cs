using MediatR;
using Orion.Core.MacroEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Application
{
    public record CalculateCurrencyFactorsCommand : IRequest<List<CurrencyFactorScore>>;
}
