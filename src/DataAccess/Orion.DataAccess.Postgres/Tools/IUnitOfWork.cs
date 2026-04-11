using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;

namespace Orion.DataAccess.Postgres.Tools
{
    public interface IUnitOfWork
    {
        IAwBuildVersionRepository AwBuildVersions { get; }
        IDatabaseLogRepository DatabaseLogs { get; }
        ITransactionHistoryArchivesRepository TransactionHistoryArchives { get; set; }
        IErrorLogsRepository ErrorLogs { get; set; }
        IShiftsRepository Shifts { get; set; }
        IDepartmentsRepository Departments { get; set; }
        IJobCandidatesRepository JobCandidates { get; set; }
        IEmployeePayHistoriesRepository EmployeePayHistories { get; set; }
        IEmployeeDepartmentHistoriesRepository EmployeeDepartmentHistories { get; set; }
        IPersonsRepository Persons { get; set; }
        IAddressesRepository Addresses { get; set; }
        IAddressTypesRepository AddressTypes { get; set; }
       
        Task<bool> SaveEntitiesAsync();
        Task<bool> SaveErrorsAsync(ErrorLog errorLogDto);
        Task StartAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> CompleteAsync();
    }

}
