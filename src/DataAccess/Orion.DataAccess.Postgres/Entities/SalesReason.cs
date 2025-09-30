using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.SalesReason")]
    [Description("Lookup table of customer purchase reasons.")]
    public class SalesReason
    {
        public SalesReason()
        {
            this.SalesOrderHeaderSalesReasons = new List<SalesOrderHeaderSalesReason>();
        }

        [Key]
        [Column(name : "SalesReasonID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Sales Reason ID is required")]
        [Display(Name = "Sales Reason ID")]
        [Description("Primary key for SalesReason records.")]
        public int? SalesReasonID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Sales reason description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ReasonType")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Reason Type is required")]
        [Display(Name = "Reason Type")]
        [Description("Category the sales reason belongs to.")]
        public string ReasonType { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.SalesOrderHeaderSalesReason.SalesReasonID -> Sales.SalesReason.SalesReasonID (FK_SalesOrderHeaderSalesReason_SalesReason_SalesReasonID)
        public IEnumerable<SalesOrderHeaderSalesReason> SalesOrderHeaderSalesReasons { get; set; }
    }
}
