using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.Culture")]
    [Description("Lookup table containing the languages in which some OrionProductionDatabase data is stored.")]
    public class Culture
    {
        public Culture()
        {
            this.ProductModelProductDescriptionCultures = new List<ProductModelProductDescriptionCulture>();
        }

        [Key]
        [Column(name : "CultureID", TypeName = "nchar")]
        [MaxLength(6)]
        [StringLength(6)]
        [Required(ErrorMessage = "Culture ID is required")]
        [Display(Name = "Culture ID")]
        [Description("Primary key for Culture records.")]
        public string CultureID { get; set; } // nchar(6)
        [Column(name : "Name")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [Description("Culture description.")]
        public string Name { get; set; } // nvarchar(50)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductModelProductDescriptionCulture.CultureID -> Production.Culture.CultureID (FK_ProductModelProductDescriptionCulture_Culture_CultureID)
        public IEnumerable<ProductModelProductDescriptionCulture> ProductModelProductDescriptionCultures { get; set; }
    }
}
