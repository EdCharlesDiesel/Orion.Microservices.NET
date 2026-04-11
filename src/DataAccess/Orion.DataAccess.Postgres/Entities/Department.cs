using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("HumanResources.Department")]
    [Description("Lookup table containing the departments within the Adventure Works Cycles company.")]
    public class Department
    {
        public Department()
        {
            this.EmployeeDepartmentHistories = new List<EmployeeDepartmentHistory>();
        }

        [Key]
        [Column(name : "DepartmentID", TypeName = "int")] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Department ID is required")]
        [Display(Name = "Department ID")]
        [Description("Primary key for Department records.")]
        public int? DepartmentID { get; set; } // smallint
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Name of the department.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "GroupName")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Group Name is required")]
        [Display(Name = "Group Name")]
        [Description("Name of the group to which the department belongs.")]
        public string GroupName { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // HumanResources.EmployeeDepartmentHistory.DepartmentID -> HumanResources.Department.DepartmentID (FK_EmployeeDepartmentHistory_Department_DepartmentID)
        public IEnumerable<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
    }
}
