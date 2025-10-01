using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    
    [Table("Production.TransactionHistoryArchive", Schema = "Production")]
    [Description("Transactions for previous years.")]
    public class TransactionHistoryArchive
    {
        [Key]
        [Column(name : "TransactionID", TypeName = "int")]
        [Required(ErrorMessage = "Transaction ID is required")]
        [Display(Name = "Transaction ID")]
        [Description("Primary key for TransactionHistoryArchive records.")]
        public int? TransactionID { get; set; } // int
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "ReferenceOrderID", TypeName = "int")]
        [Required(ErrorMessage = "Reference Order ID is required")]
        [Display(Name = "Reference Order ID")]
        [Description("Purchase order, sales order, or work order identification number.")]
        public int? ReferenceOrderID { get; set; } // int
        [Column(name : "ReferenceOrderLineID", TypeName = "int")]
        [Required(ErrorMessage = "Reference Order Line ID is required")]
        [Display(Name = "Reference Order Line ID")]
        [Description("Line number associated with the purchase order, sales order, or work order.")]
        public int? ReferenceOrderLineID { get; set; } // int
        [Column(name : "TransactionDate")]
        [Required(ErrorMessage = "Transaction Date is required")]
        [Display(Name = "Transaction Date")]
        [Description("Date and time of the transaction.")]
        public DateTime? TransactionDate { get; set; } // datetime
        [Column(name : "TransactionType", TypeName = "nchar")]
        [MaxLength(1)]
        [StringLength(1)]
        [Required(ErrorMessage = "Transaction Type is required")]
        [Display(Name = "Transaction Type")]
        [Description("W = Work Order, S = Sales Order, P = Purchase Order")]
        public string TransactionType { get; set; } // nchar(1)
        [Column(name : "Quantity", TypeName = "int")]
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Description("Product quantity.")]
        public int? Quantity { get; set; } // int
        [Column(name : "ActualCost", TypeName = "money")]
        [Required(ErrorMessage = "Actual Cost is required")]
        [Display(Name = "Actual Cost")]
        [Description("Product cost.")]
        public decimal? ActualCost { get; set; } // money
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime
    }
}
