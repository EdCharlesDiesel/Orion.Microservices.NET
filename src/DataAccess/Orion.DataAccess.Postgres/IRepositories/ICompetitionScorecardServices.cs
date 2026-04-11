using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orion.Domain.IRepositories;

public interface ICompetitionScorecardServices:IRepository<CompetitionMatch>
{
    Task<CompetitionMatch> BuildCreate(List<CompetitionMatch> coupons);
}

public class CompetitionMatch
{
}