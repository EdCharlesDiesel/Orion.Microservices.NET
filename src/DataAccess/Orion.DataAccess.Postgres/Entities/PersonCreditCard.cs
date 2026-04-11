using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.PersonCreditCard")]
    [Description("Cross-reference table mapping people to their credit card information in the CreditCard table. ")]
    public class PersonCreditCard
    {
        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int", Order = 1)]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Business entity identification number. Foreign key to Person.BusinessEntityID.")]
        public int? BusinessEntityID { get; set; } // int
        [Key]
        [Column(name : "CreditCardID", TypeName = "int", Order = 2)]
        [Required(ErrorMessage = "Credit Card ID is required")]
        [Display(Name = "Credit Card ID")]
        [Description("Credit card identification number. Foreign key to CreditCard.CreditCardID.")]
        public int? CreditCardID { get; set; } // int
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.PersonCreditCard.BusinessEntityID -> Person.Person.BusinessEntityID (FK_PersonCreditCard_Person_BusinessEntityID)
        [ForeignKey("BusinessEntityID")]
        public Person Person { get; set; }
        // Sales.PersonCreditCard.CreditCardID -> Sales.CreditCard.CreditCardID (FK_PersonCreditCard_CreditCard_CreditCardID)
        [ForeignKey("CreditCardID")]
        public CreditCard CreditCard { get; set; }
    }
}
