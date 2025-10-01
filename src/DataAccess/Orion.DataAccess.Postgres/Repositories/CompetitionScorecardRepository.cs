using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.Shared;

namespace Orion.DataAccess.Postgres.Repositories;

public class CompetitionScorecardRepository(OrionDbContext context) : Domain.IRepositories.IRepository<CompetitionMatch>
{
    // public async Task<IEnumerable<CompetitionMatch>> GetAllAsync()
    // {
    //     var coupons = context.CompetitionMatches.ToList();
    //     if (coupons == null || !coupons.Any())
    //         throw new ArgumentException("coupons be null or empty.");
    //
    //     return coupons.ToList();
    // }
    //
    // public async Task<List<CompetitionMatch>> CreateCompetitionMatchs(List<CompetitionMatch> coupons)
    // {
    //     if (coupons == null)
    //         throw new ArgumentException("coupon be null or empty.");
    //
    //     await context.CompetitionMatches.AddRangeAsync(coupons);
    //     await context.SaveChangesAsync();
    //
    //     return coupons;
    // }
    //
    // //TODO: Add comments
    // public async Task<CompetitionMatch> Create(List<CompetitionMatch> coupons)
    // {
    //     if (coupons == null || !coupons.Any())
    //         throw new ArgumentException("coupon be null or empty.");
    //
    //     await context.CompetitionMatches.AddRangeAsync(coupons);
    //     await context.SaveChangesAsync();
    //
    //     // Return the first created event (or you can change this to return the list)
    //     return coupons.First();
    // }
    //
    // public async Task<CompetitionMatch?> GetByIdAsync(Guid id)
    // {
    //     var coupon = context.CompetitionMatches.FirstOrDefault(x => x.Id == id);
    //     if (coupon == null)
    //         throw new ArgumentException("coupon id cannot be null or empty.");
    //
    //     return coupon;
    // }
    //
    // public async Task AddAsync(CompetitionMatch coupon)
    // {
    //     if (coupon == null)
    //         throw new ArgumentException("CompetitionMatch cannot be null or empty.");
    //
    //     await context.CompetitionMatches.AddAsync(coupon);
    //     await context.SaveChangesAsync();
    // }
    //
    // public async Task UpdateAsync(CompetitionMatch entity)
    // {
    //     var coupon = context.CompetitionMatches.FirstOrDefault(x => x.Id == entity.Id);
    //     if (coupon == null)
    //         throw new ArgumentException("CompetitionMatch cannot be null or empty.");
    //
    //     context.CompetitionMatches.Update(coupon);
    //     await context.SaveChangesAsync();
    // }
    //
    //
    // public async Task DeleteAsync(Guid id)
    // {
    //     var coupon = context.CompetitionMatches.FirstOrDefault(x => x.Id == id);
    //     if (coupon == null)
    //         throw new ArgumentException("CompetitionMatch cannot be null or empty.");
    //
    //     context.CompetitionMatches.Remove(coupon);
    //     await context.SaveChangesAsync();
    // }
    //
    // public async Task BulkCreate(List<CompetitionMatch> coupons)
    // {
    //     if (coupons == null)
    //         throw new ArgumentException("CompetitionMatches cannot be null or empty.");
    //
    //     await context.CompetitionMatches.AddRangeAsync(coupons);
    //     await context.SaveChangesAsync();
    // }
    public async Task<IEnumerable<CompetitionMatch>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(CompetitionMatch entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(CompetitionMatch entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}