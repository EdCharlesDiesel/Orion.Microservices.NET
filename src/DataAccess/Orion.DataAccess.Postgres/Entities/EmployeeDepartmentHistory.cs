using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("EmployeeDepartmentHistory",Schema = "HumanResources")]
    [Description("Employee department transfers.")]
    public class EmployeeDepartmentHistory
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Employee identification number. Foreign key to Employee.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "DepartmentID", TypeName = "smallint", Order = 3)]
        [Required(ErrorMessage = "Department ID is required")]
        [Display(Name = "Department ID")]
        [Description("Department in which the employee worked including currently. Foreign key to Department.DepartmentID.")]
        public int? DepartmentID { get; set; } // smallint
        [Key]
        [Column(name : "ShiftID", Order = 4)]
        [Required(ErrorMessage = "Shift ID is required")]
        [Display(Name = "Shift ID")]
        [Description("Identifies which 8-hour shift the employee works. Foreign key to Shift.Shift.ID.")]
        public int? ShiftID { get; set; } // tinyint
        [Key]
        [Column(name : "StartDate", TypeName = "date", Order = 2)]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [Description("Date the employee started work in the department.")]
        public DateTime? StartDate { get; set; } // date
        [Column(name : "EndDate", TypeName = "date")]
        [Display(Name = "End Date")]
        [Description("Date the employee left the department. NULL = Current department.")]
        public DateTime? EndDate { get; set; } // date
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // HumanResources.EmployeeDepartmentHistory.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_EmployeeDepartmentHistory_Employee_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
        // HumanResources.EmployeeDepartmentHistory.DepartmentID -> HumanResources.Department.DepartmentID (FK_EmployeeDepartmentHistory_Department_DepartmentID)
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        // HumanResources.EmployeeDepartmentHistory.ShiftID -> HumanResources.Shift.ShiftID (FK_EmployeeDepartmentHistory_Shift_ShiftID)
        [ForeignKey("ShiftID")]
        public Shift Shift { get; set; }
    }
}
