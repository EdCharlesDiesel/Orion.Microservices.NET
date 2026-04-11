using Orion.Admin.Models.Employees;
using Orion.Admin.Tools;

namespace Orion.Admin.Queries
{
    public interface IEmployeesListQuery: IQuery
    {
        Task<IEnumerable<EmployeeInfosViewModel>> GetAllEmployees();        
    }
}
