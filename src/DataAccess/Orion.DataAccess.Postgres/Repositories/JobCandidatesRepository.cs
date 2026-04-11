using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Repositories;

public class JobCandidatesRepository(OrionDbContext context): IJobCandidatesRepository
{
    public async Task<IEnumerable<JobCandidate>> GetAllAsync() =>
        await context.JobCandidates.ToListAsync();

    public async Task<JobCandidate?> GetByIdAsync(int id) =>
        await context.JobCandidates.FindAsync(id);

    public async Task AddAsync(JobCandidate entity) =>
        await context.JobCandidates.AddAsync(entity);

    public void Update(JobCandidate entity) =>
        context.JobCandidates.Update(entity);

    public void Delete(JobCandidate entity) =>
        context.JobCandidates.Remove(entity);
}