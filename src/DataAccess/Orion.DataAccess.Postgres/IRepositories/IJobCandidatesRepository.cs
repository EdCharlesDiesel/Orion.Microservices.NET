using Orion.DataAccess.Postgres.Entities;

namespace Orion.DataAccess.Postgres.IRepositories;

public interface IJobCandidatesRepository
{
    Task<IEnumerable<JobCandidate>> GetAllAsync();
    Task<JobCandidate?> GetByIdAsync(int id);
    Task AddAsync(JobCandidate entity);
    void Update(JobCandidate entity);
    void Delete(JobCandidate entity);
}