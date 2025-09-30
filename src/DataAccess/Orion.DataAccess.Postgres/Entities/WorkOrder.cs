using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.WorkOrder")]
    [Description("Manufacturing work orders.")]
    public class WorkOrder
    {
        public WorkOrder()
        {
            this.WorkOrderRoutings = new List<WorkOrderRouting>();
        }

        [Key]
        [Column(name : "WorkOrderID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Work Order ID is required")]
        [Display(Name = "Work Order ID")]
        [Description("Primary key for WorkOrder records.")]
        public int? WorkOrderID { get; set; } // int
        [Column(name : "ProductID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        [Display(Name = "Product ID")]
        [Description("Product identification number. Foreign key to Product.ProductID.")]
        public int? ProductID { get; set; } // int
        [Column(name : "OrderQty", TypeName = "int")]
        [Required(ErrorMessage = "Order Qty is required")]
        [Display(Name = "Order Qty")]
        [Description("Product quantity to build.")]
        public int? OrderQty { get; set; } // int
        [Column(name : "StockedQty", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required(ErrorMessage = "Stocked Qty is required")]
        [Display(Name = "Stocked Qty")]
        [Description("Quantity built and put in inventory.")]
        public int? StockedQty { get; set; } // int
        [Column(name : "ScrappedQty", TypeName = "smallint")]
        [Required(ErrorMessage = "Scrapped Qty is required")]
        [Display(Name = "Scrapped Qty")]
        [Description("Quantity that failed inspection.")]
        public short? ScrappedQty { get; set; } // smallint
        [Column(name : "StartDate")]
        [Required(ErrorMessage = "Start Date is required")]
        [Display(Name = "Start Date")]
        [Description("Work order start date.")]
        public DateTime? StartDate { get; set; } // datetime
        [Column(name : "EndDate")]
        [Display(Name = "End Date")]
        [Description("Work order end date.")]
        public DateTime? EndDate { get; set; } // datetime
        [Column(name : "DueDate")]
        [Required(ErrorMessage = "Due Date is required")]
        [Display(Name = "Due Date")]
        [Description("Work order due date.")]
        public DateTime? DueDate { get; set; } // datetime
        [Column(name : "ScrapReasonID", TypeName = "smallint")]
        [Display(Name = "Scrap Reason ID")]
        [Description("Reason for inspection failure.")]
        public short? ScrapReasonID { get; set; } // smallint
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.WorkOrder.ProductID -> Production.Product.ProductID (FK_WorkOrder_Product_ProductID)
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        // Production.WorkOrder.ScrapReasonID -> Production.ScrapReason.ScrapReasonID (FK_WorkOrder_ScrapReason_ScrapReasonID)
        [ForeignKey("ScrapReasonID")]
        public ScrapReason ScrapReason { get; set; }
        // Production.WorkOrderRouting.WorkOrderID -> Production.WorkOrder.WorkOrderID (FK_WorkOrderRouting_WorkOrder_WorkOrderID)
        public IEnumerable<WorkOrderRouting> WorkOrderRoutings { get; set; }
    }
}
