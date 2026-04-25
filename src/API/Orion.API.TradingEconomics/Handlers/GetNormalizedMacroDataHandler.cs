using MediatR;
using Orion.API.TradingEconomics.Application;
using Orion.API.TradingEconomics.Entities;
using Orion.API.TradingEconomics.Interfaces;

namespace Orion.API.TradingEconomics.Handlers
{


    public class GetNormalizedMacroDataHandler: IRequestHandler<GetNormalizedMacroDataQuery, List<NormalizedIndicator>>
    {
        private readonly IRepository<NormalizedIndicator> _repo;

        public GetNormalizedMacroDataHandler(IRepository<NormalizedIndicator> repo)
        {
            _repo = repo;
        }

        public async Task<List<NormalizedIndicator>> Handle(
            GetNormalizedMacroDataQuery request,
            CancellationToken ct)
        {
            var data = await _repo.GetAllAsync();

            // Optional filtering
            if (!string.IsNullOrWhiteSpace(request.Country))
            {
                data = data.Where(x => x.Country == request.Country);
            }

            if (!string.IsNullOrWhiteSpace(request.Indicator))
            {
                data = data.Where(x => x.Indicator.Contains(request.Indicator));
            }

            // IMPORTANT: Only return latest per (Country + Indicator)
            var latest = data
                .GroupBy(x => new { x.Country, x.Indicator })
                .Select(g => g.OrderByDescending(x => x.Date).First())
                .ToList();

            return latest;
        }
    }
}
