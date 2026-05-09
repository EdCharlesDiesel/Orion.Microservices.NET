using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{

    public record GenerateFxSignalsCommand : IRequest<List<FxSignal>>;
}
