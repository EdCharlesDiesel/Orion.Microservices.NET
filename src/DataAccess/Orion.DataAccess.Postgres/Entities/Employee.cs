using Microsoft.SqlServer.Types;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("HumanResources.Employee")]
    [Description("Employee information such as salary, department, and title.")]
    public class Employee
    {
        public Employee()
        {
            this.EmployeeDepartmentHistories = new List<EmployeeDepartmentHistory>();
            this.EmployeePayHistories = new List<EmployeePayHistory>();
            this.JobCandidates = new List<JobCandidate>();
            this.Documents = new List<Document>();
            this.PurchaseOrderHeaders = new List<PurchaseOrderHeader>();
            
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for Employee records.  Foreign key to BusinessEntity.BusinessEntityID.")]
        public int BusinessEntityID { get; set; } // int
        [Column(name : "NationalIDNumber")]
        [MaxLength(15)]
        [StringLength(15)]
        [Required(ErrorMessage = "National ID Number is required")]
        [Display(Name = "National ID Number")]
        [Description("Unique national identification number such as a social security number.")]
        public string NationalIDNumber { get; set; } // nvarchar(15)
        [Column(name : "LoginID")]
        [MaxLength(256)]
        [StringLength(256)]
        [Required(ErrorMessage = "Login ID is required")]
        [Display(Name = "Login ID")]
        [Description("Network login.")]
        public string LoginID { get; set; } // nvarchar(256)
        [Column(name : "OrganizationNode", TypeName = "hierarchyid")]
        [Display(Name = "Organization Node")]
        [Description("Where the employee is located in corporate hierarchy.")]
        [NotMapped]
        public SqlHierarchyId OrganizationNode { get; set; } // hierarchyid
        [Column(name : "OrganizationLevel", TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Organization Level")]
        [Description("The depth of the employee in the corporate hierarchy.")]
        public short? OrganizationLevel { get; set; } // smallint
        [Column(name : "JobTitle")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]
        [Description("Work title such as Buyer or Sales Representative.")]
        public string JobTitle { get; set; } // nvarchar(50)
        [Column(name : "BirthDate", TypeName = "date")]
        [Required(ErrorMessage = "Birth Date is required")]
        [Display(Name = "Birth Date")]
        [Description("Date of birth.")]
        public DateTime? BirthDate { get; set; } // date
        [Column(name : "MaritalStatus", TypeName = "nchar")]
        [MaxLength(1)]
        [StringLength(1)]
        [Required(ErrorMessage = "Marital Status is required")]
        [Display(Name = "Marital Status")]
        [Description("M = Married, S = Single")]
        public string MaritalStatus { get; set; } // nchar(1)
        [Column(name : "Gender", TypeName = "nchar")]
        [MaxLength(1)]
        [StringLength(1)]
        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        [Description("M = Male, F = Female")]
        public string Gender { get; set; } // nchar(1)
        [Column(name : "HireDate", TypeName = "date")]
        [Required(ErrorMessage = "Hire Date is required")]
        [Display(Name = "Hire Date")]
        [Description("Employee hired on this date.")]
        public DateTime? HireDate { get; set; } // date
        [Column(name : "SalariedFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Salaried Flag is required")]
        [Display(Name = "Salaried Flag")]
        [Description("Job classification. 0 = Hourly, not exempt from collective bargaining. 1 = Salaried, exempt from collective bargaining.")]
        [NotMapped]
        public bool? SalariedFlag { get; set; } // bit
        [Column(name : "VacationHours", TypeName = "smallint")]
        [Required(ErrorMessage = "Vacation Hours is required")]
        [Display(Name = "Vacation Hours")]
        [Description("Number of available vacation hours.")]
        public short? VacationHours { get; set; } // smallint
        [Column(name : "SickLeaveHours", TypeName = "smallint")]
        [Required(ErrorMessage = "Sick Leave Hours is required")]
        [Display(Name = "Sick Leave Hours")]
        [Description("Number of available sick leave hours.")]
        public short? SickLeaveHours { get; set; } // smallint
        [Column(name : "CurrentFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Current Flag is required")]
        [Display(Name = "Current Flag")]
        [Description("0 = Inactive, 1 = Active")]
        [NotMapped]
        public bool? CurrentFlag { get; set; } // bit
        [Column(name : "rowguid")]
        [Required(ErrorMessage = "rowguid is required")]
        [Display(Name = "rowguid")]
        [Description("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")]
        public Guid? rowguid { get; set; } // uniqueidentifier
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // HumanResources.Employee.BusinessEntityID -> Person.Person.BusinessEntityID (FK_Employee_Person_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Person? Person { get; set; }
        // HumanResources.EmployeeDepartmentHistory.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_EmployeeDepartmentHistory_Employee_BusinessEntityID)
        public IEnumerable<EmployeeDepartmentHistory> EmployeeDepartmentHistories { get; set; }
        // HumanResources.EmployeePayHistory.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_EmployeePayHistory_Employee_BusinessEntityID)
        public IEnumerable<EmployeePayHistory> EmployeePayHistories { get; set; }
        // HumanResources.JobCandidate.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_JobCandidate_Employee_BusinessEntityID)
        public IEnumerable<JobCandidate> JobCandidates { get; set; }
        // Production.Document.Owner -> HumanResources.Employee.BusinessEntityID (FK_Document_Employee_Owner)
        public IEnumerable<Document> Documents { get; set; }
        // Purchasing.PurchaseOrderHeader.EmployeeID -> HumanResources.Employee.BusinessEntityID (FK_PurchaseOrderHeader_Employee_EmployeeID)
        public IEnumerable<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        // Sales.SalesPerson.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_SalesPerson_Employee_BusinessEntityID)
        public SalesPerson? SalesPerson { get; set; }
        public int JobLevel { get; set; }
        public int YearsInService { get; set; }
        public IEnumerable<Course> AttendedCourses { get; set; }
        public int SuggestedBonus { get; set; }
        public int Salary { get; set; }
        public bool MinimumRaiseGiven { get; set; }
        public Guid Id { get; set; }
        public int EntityVersion { get; set; }
    }
}
