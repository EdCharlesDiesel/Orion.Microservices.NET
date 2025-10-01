using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    
    [Table("Sales.Store", Schema = "Sales")]
    [Description("Customers (resellers) of Adventure Works products.")]
    public class Store
    {
        public Store()
        {
            this.Customers = new List<Customer>();
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key. Foreign key to Customer.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Name of the store.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "SalesPersonID", TypeName = "int")]
        [Display(Name = "Sales Person ID")]
        [Description("ID of the sales person assigned to the customer. Foreign key to SalesPerson.BusinessEntityID.")]
        public int? SalesPersonID { get; set; } // int
        [Column(name : "Demographics", TypeName = "xml")]
        [Display(Name = "Demographics")]
        [Description("Demographic informationg about the store such as the number of employees, annual sales and store type.")]
        public string Demographics { get; set; } // XML(.)
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

        // Sales.Store.BusinessEntityID -> Person.BusinessEntity.BusinessEntityID (FK_Store_BusinessEntity_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public BusinessEntity BusinessEntity { get; set; }
        // Sales.Store.SalesPersonID -> Sales.SalesPerson.BusinessEntityID (FK_Store_SalesPerson_SalesPersonID)
        [ForeignKey("BusinessEntityID")]
        public SalesPerson SalesPerson { get; set; }
        // Sales.Customer.StoreID -> Sales.Store.BusinessEntityID (FK_Customer_Store_StoreID)
        public IEnumerable<Customer> Customers { get; set; }
    }
}
