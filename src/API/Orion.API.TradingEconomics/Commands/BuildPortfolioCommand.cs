using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Commands
{
    public record BuildPortfolioCommand(decimal Capital) : IRequest<List<PortfolioPosition>>;
}
