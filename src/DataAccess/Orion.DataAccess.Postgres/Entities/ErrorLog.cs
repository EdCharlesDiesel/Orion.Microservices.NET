using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("ErrorLog")]
    [Description("Audit table tracking errors in the the OrionProductionDatabase database that are caught by the CATCH block of a TRY...CATCH construct. Data is inserted by stored procedure dbo.uspLogError when it is executed from inside the CATCH block of a TRY...CATCH construct.")]
    public class ErrorLog
    {
        [Key]
        [Column(name : "ErrorLogID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Error Log ID is required")]
        [Display(Name = "Error Log ID")]
        [Description("Primary key for ErrorLog records.")]
        public int? ErrorLogID { get; set; } // int
        [Column(name : "ErrorTime")]
        [Required(ErrorMessage = "Error Time is required")]
        [Display(Name = "Error Time")]
        [Description("The date and time at which the error occurred.")]
        public DateTimeOffset? ErrorTime { get; set; } // datetime
        [Column(name : "UserName")]
        [MaxLength(128)]
        [StringLength(128)]
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name")]
        [Description("The user who executed the batch in which the error occurred.")]
        public string UserName { get; set; } // nvarchar(128)
        [Column(name : "ErrorNumber", TypeName = "int")]
        [Required(ErrorMessage = "Error Number is required")]
        [Display(Name = "Error Number")]
        [Description("The error number of the error that occurred.")]
        public int? ErrorNumber { get; set; } // int
        [Column(name : "ErrorSeverity", TypeName = "int")]
        [Display(Name = "Error Severity")]
        [Description("The severity of the error that occurred.")]
        public int? ErrorSeverity { get; set; } // int
        [Column(name : "ErrorState", TypeName = "int")]
        [Display(Name = "Error State")]
        [Description("The state number of the error that occurred.")]
        public int? ErrorState { get; set; } // int
        [Column(name : "ErrorProcedure")]
        [MaxLength(126)]
        [StringLength(126)]
        [Display(Name = "Error Procedure")]
        [Description("The name of the stored procedure or trigger where the error occurred.")]
        public string? ErrorProcedure { get; set; } // nvarchar(126)
        [Column(name : "ErrorLine", TypeName = "int")]
        [Display(Name = "Error Line")]
        [Description("The line number at which the error occurred.")]
        public int? ErrorLine { get; set; } // int
        [Column(name : "ErrorMessage")]
        [MaxLength(4000)]
        [StringLength(4000)]
        [Required(ErrorMessage = "Error Message is required")]
        [Display(Name = "Error Message")]
        [Description("The message text of the error that occurred.")]
        public string ErrorMessage { get; set; } // nvarchar(4000)
    }
}
