using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Sales.CurrencyRate")]
    [Description("Currency exchange rates.")]
    public class CurrencyRate
    {
        public CurrencyRate()
        {
            this.SalesOrderHeaders = new List<SalesOrderHeader>();
        }

        [Key]
        [Column(name : "CurrencyRateID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Currency Rate ID is required")]
        [Display(Name = "Currency Rate ID")]
        [Description("Primary key for CurrencyRate records.")]
        public int? CurrencyRateID { get; set; } // int
        [Column(name : "CurrencyRateDate")]
        [Required(ErrorMessage = "Currency Rate Date is required")]
        [Display(Name = "Currency Rate Date")]
        [Description("Date and time the exchange rate was obtained.")]
        public DateTime? CurrencyRateDate { get; set; } // datetime
        [Column(name : "FromCurrencyCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "From Currency Code is required")]
        [Display(Name = "From Currency Code")]
        [Description("Exchange rate was converted from this currency code.")]
        public string FromCurrencyCode { get; set; } // nchar(3)
        [Column(name : "ToCurrencyCode", TypeName = "nchar")]
        [MaxLength(3)]
        [StringLength(3)]
        [Required(ErrorMessage = "To Currency Code is required")]
        [Display(Name = "To Currency Code")]
        [Description("Exchange rate was converted to this currency code.")]
        public string ToCurrencyCode { get; set; } // nchar(3)
        [Column(name : "AverageRate", TypeName = "money")]
        [Required(ErrorMessage = "Average Rate is required")]
        [Display(Name = "Average Rate")]
        [Description("Average exchange rate for the day.")]
        public decimal? AverageRate { get; set; } // money
        [Column(name : "EndOfDayRate", TypeName = "money")]
        [Required(ErrorMessage = "End Of Day Rate is required")]
        [Display(Name = "End Of Day Rate")]
        [Description("Final exchange rate for the day.")]
        public decimal? EndOfDayRate { get; set; } // money
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Sales.CurrencyRate.FromCurrencyCode -> Sales.Currency.CurrencyCode (FK_CurrencyRate_Currency_FromCurrencyCode)
        [ForeignKey("FromCurrencyCode")]
        public Currency Currency { get; set; }
        // Sales.CurrencyRate.ToCurrencyCode -> Sales.Currency.CurrencyCode (FK_CurrencyRate_Currency_ToCurrencyCode)
        [ForeignKey("ToCurrencyCode")]
        public Currency Currency1 { get; set; }
        // Sales.SalesOrderHeader.CurrencyRateID -> Sales.CurrencyRate.CurrencyRateID (FK_SalesOrderHeader_CurrencyRate_CurrencyRateID)
        public IEnumerable<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
