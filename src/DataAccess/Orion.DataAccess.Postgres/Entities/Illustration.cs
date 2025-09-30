using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.Illustration")]
    [Description("Bicycle assembly diagrams.")]
    public class Illustration
    {
        public Illustration()
        {
            this.ProductModelIllustrations = new List<ProductModelIllustration>();
        }

        [Key]
        [Column(name : "IllustrationID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Illustration ID is required")]
        [Display(Name = "Illustration ID")]
        [Description("Primary key for Illustration records.")]
        public int? IllustrationID { get; set; } // int
        [Column(name : "Diagram", TypeName = "xml")]
        [Display(Name = "Diagram")]
        [Description("Illustrations used in manufacturing instructions. Stored as XML.")]
        public string Diagram { get; set; } // XML(.)
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.ProductModelIllustration.IllustrationID -> Production.Illustration.IllustrationID (FK_ProductModelIllustration_Illustration_IllustrationID)
        public IEnumerable<ProductModelIllustration> ProductModelIllustrations { get; set; }
    }
}
