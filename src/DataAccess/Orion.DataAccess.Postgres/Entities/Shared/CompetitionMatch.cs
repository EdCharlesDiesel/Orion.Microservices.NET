using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Shared
{
    public class CompetitionMatch:Entity<Guid>
    {
        private string? PlayerOne { get; set; }
        public string PlayerTwo { get; set; } = null!;
        public Guid LeagueCode { get; set; }
    }
}
