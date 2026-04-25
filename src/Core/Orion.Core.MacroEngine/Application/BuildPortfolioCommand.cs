using MediatR;
using Orion.Core.MacroEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Application
{
    public record BuildPortfolioCommand(double Capital)        : IRequest<List<PortfolioPosition>>;
}
