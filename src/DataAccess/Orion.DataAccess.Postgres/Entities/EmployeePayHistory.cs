using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("HumanResources.EmployeePayHistory")]
    [Description("Employee pay history.")]
    public class EmployeePayHistory
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Employee identification number. Foreign key to Employee.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "RateChangeDate", Order = 2)]
        [Required(ErrorMessage = "Rate Change Date is required")]
        [Display(Name = "Rate Change Date")]
        [Description("Date the change in pay is effective")]
        public DateTime? RateChangeDate { get; set; } // datetime
        [Column(name : "Rate", TypeName = "money")]
        [Required(ErrorMessage = "Rate is required")]
        [Display(Name = "Rate")]
        [Description("Salary hourly rate.")]
        public decimal? Rate { get; set; } // money
        [Column(name : "PayFrequency")]
        [Required(ErrorMessage = "Pay Frequency is required")]
        [Display(Name = "Pay Frequency")]
        [Description("1 = Salary received monthly, 2 = Salary received biweekly")]
        public byte? PayFrequency { get; set; } // tinyint
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // HumanResources.EmployeePayHistory.BusinessEntityID -> HumanResources.Employee.BusinessEntityID (FK_EmployeePayHistory_Employee_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
    }
}
