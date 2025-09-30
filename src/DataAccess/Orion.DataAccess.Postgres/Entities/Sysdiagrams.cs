using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("sysdiagrams")]
    public class Sysdiagrams
    {
        [Column(name : "name")]
        [MaxLength(128)]
        [StringLength(128)]
        [Required(ErrorMessage = "name is required")]
        [Display(Name = "name")]
        public string name { get; set; } // nvarchar(128)
        [Column(name : "principal_id", TypeName = "int")]
        [Required(ErrorMessage = "principal id is required")]
        [Display(Name = "principal id")]
        public int? principal_id { get; set; } // int
        [Key]
        [Column(name : "diagram_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "diagram id is required")]
        [Display(Name = "diagram id")]
        public int? diagram_id { get; set; } // int
        [Column(name : "version", TypeName = "int")]
        [Display(Name = "version")]
        public int? version { get; set; } // int
        [Column(name : "definition")]
        [MaxLength]
        [Display(Name = "definition")]
        public byte[] definition { get; set; } // varbinary(max)
    }
}
