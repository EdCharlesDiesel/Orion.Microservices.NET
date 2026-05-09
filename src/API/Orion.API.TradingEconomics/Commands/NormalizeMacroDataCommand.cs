using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{
    public record NormalizeMacroDataCommand(bool ForceRefresh = false) : IRequest<int>;
}