using MediatR;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Application
{
    public record GetNormalizedMacroDataQuery(
        string? Country = null,
        string? Indicator = null
    ) : IRequest<List<NormalizedIndicator>>;
}
