namespace Orion.API.Dashboard.Business.EventArguments
{
    public class EmployeeIsAbsentEventArgs : EventArgs
    {
        public int EmployeeId { get; private set; }

        public EmployeeIsAbsentEventArgs(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
