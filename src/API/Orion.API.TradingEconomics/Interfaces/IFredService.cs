using Orion.API.TradingEconomics.DTO;
using Orion.API.TradingEconomics.Entities;

namespace Orion.API.TradingEconomics.Interfaces
{
    public interface IFredService
    {
        Task<MacroData> GetMacroDataAsync(CancellationToken cancellationToken = default);        
        Task<MacroData> RefreshMacroDataAsync( CancellationToken cancellationToken = default);
        Dictionary<string, Dictionary<string, string>> GetFredSeriesMappings();
        Task<FredStatusResponse> CheckStatusAsync( CancellationToken cancellationToken = default);
    }
}
