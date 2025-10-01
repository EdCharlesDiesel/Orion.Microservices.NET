
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities.Shared
{
    public class EmployeeAddress:Entity<Guid>
    {
        public int EmployeeId { get; set; }
        public int AddressId { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Address Address { get; set; }
        public Employee Employee { get; set; }
    }
}
