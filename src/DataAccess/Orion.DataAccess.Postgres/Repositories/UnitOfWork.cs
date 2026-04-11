

using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.IRepositories;
using Orion.DataAccess.Postgres.Tools;
using Serilog;



namespace Orion.DataAccess.Postgres.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrionDbContext _context;

        public UnitOfWork(OrionDbContext context)
        {
            _context = context;
            AwBuildVersions = new AwBuildVersionRepository(_context);
            DatabaseLogs = new DatabaseLogRepository(_context);
            TransactionHistoryArchives = new TransactionHistoryArchivesRepository(_context);
            ErrorLogs = new ErrorLogsRepository(_context);
            Shifts = new ShiftsRepository(_context);
            Departments = new DepartmentsRepository(_context);
            JobCandidates = new  JobCandidatesRepository(_context);
            EmployeePayHistories = new EmployeePayHistoriesRepository(_context);
            EmployeeDepartmentHistories = new  EmployeeDepartmentHistoriesRepository(_context);
            Persons = new  PersonsRepository(_context);
            Addresses = new  AddressesRepository(_context); 
            AddressTypes = new  AddressTypesRepository(_context); 
        }

        public IAwBuildVersionRepository AwBuildVersions { get; set; }
        public IDatabaseLogRepository DatabaseLogs { get; set; }
        public ITransactionHistoryArchivesRepository TransactionHistoryArchives { get; set; }
        public IErrorLogsRepository ErrorLogs { get; set; }
        public IShiftsRepository Shifts { get; set; }
        public IDepartmentsRepository Departments { get; set; }
        public IJobCandidatesRepository JobCandidates { get; set; }
        public IEmployeePayHistoriesRepository EmployeePayHistories { get; set; }
        public IEmployeeDepartmentHistoriesRepository EmployeeDepartmentHistories { get; set; }
        public IPersonsRepository Persons { get; set; }
        public IAddressesRepository Addresses { get; set; }
        public IAddressTypesRepository AddressTypes { get; set; }
        public Task StartAsync() => Task.CompletedTask;

        public Task CommitAsync() => Task.CompletedTask;

        public Task RollbackAsync() => Task.CompletedTask;
        
        public async Task<bool> SaveEntitiesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> SaveErrorsAsync(ErrorLog errorLog)
        {
            try
            {
                await _context.ErrorLogs.AddAsync(errorLog);
                var result = await _context.SaveChangesAsync();

                // Log with Serilog
                Log.Error("Database ErrorLog saved: {@ErrorLog}", errorLog);

                return result > 0;
            }
            catch (Exception ex)
            {
                // Log failure with Serilog
                Log.Error(ex, "Failed to save ErrorLog: {@ErrorLog}", errorLog);
                return false;
            }
        }


        public async Task<int> CompleteAsync() =>
            await _context.SaveChangesAsync();
        
        public void Dispose() => _context.Dispose();
    }


}