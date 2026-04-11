using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.CreditCard")]
    [Description("Customer credit card information.")]
    public class CreditCard
    {
        public CreditCard()
        {
            this.PersonCreditCards = new List<PersonCreditCard>();
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
        }

        [Key]
        [Column(name : "CreditCardID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Credit Card ID is required")]
        [Display(Name = "Credit Card ID")]
        [Description("Primary key for CreditCard records.")]
        public int? CreditCardID { get; set; } // int
        [Column(name : "CardType")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Card Type is required")]
        [Display(Name = "Card Type")]
        [Description("Credit card name.")]
        public string CardType { get; set; } // nvarchar(50)
        [Column(name : "CardNumber")]
        [MaxLength(25)]
        [StringLength(25)]
        [Required(ErrorMessage = "Card Number is required")]
        [Display(Name = "Card Number")]
        [Description("Credit card number.")]
        public string CardNumber { get; set; } // nvarchar(25)
        [Column(name : "ExpMonth")]
        [Required(ErrorMessage = "Exp Month is required")]
        [Display(Name = "Exp Month")]
        [Description("Credit card expiration month.")]
        public byte? ExpMonth { get; set; } // tinyint
        [Column(name : "ExpYear", TypeName = "smallint")]
        [Required(ErrorMessage = "Exp Year is required")]
        [Display(Name = "Exp Year")]
        [Description("Credit card expiration year.")]
        public short? ExpYear { get; set; } // smallint
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.PersonCreditCard.CreditCardID -> Sales.CreditCard.CreditCardID (FK_PersonCreditCard_CreditCard_CreditCardID)
        public IEnumerable<PersonCreditCard> PersonCreditCards { get; set; }
        // Sales.SalesOrderHeader.CreditCardID -> Sales.CreditCard.CreditCardID (FK_SalesOrderHeader_CreditCard_CreditCardID)
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
