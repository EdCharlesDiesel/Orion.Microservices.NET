using MediatR;

namespace Orion.API.TradingEconomics.Commands
{

        public record IngestMacroDataCommand(string Country) : IRequest<int>;
    
}
