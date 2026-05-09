using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.MacroEngine.Entities
{

    public record GenerateFxSignalsCommand : IRequest<List<FxSignal>>;
}
