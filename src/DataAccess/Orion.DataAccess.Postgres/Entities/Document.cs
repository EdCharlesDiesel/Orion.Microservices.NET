using Microsoft.SqlServer.Types;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Tools;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Production.Document")]
    [Description("Product maintenance documents.")]
    public class Document
    {
        public Document()
        {
            this.ProductDocuments = new List<ProductDocument>();
        }

        [Key]
        [Column(name : "BusinessEntityID", TypeName = "int")]
        [Required(ErrorMessage = "Business Entity ID is required")]
        [Display(Name = "Business Entity ID")]
        [Description("Primary key for Document records.  Foreign key to BusinessEntity.BusinessEntityID.")]
        public int BusinessEntityID { get; set; } // int
        [Column(name : "DocumentLevel", TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Document Level")]
        [Description("Depth in the document hierarchy.")]
        public short? DocumentLevel { get; set; } // smallint
        [Column(name : "Title")]
        [MaxLength(50)]
        [StringLength(50)]
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        [Description("Title of the document.")]
        public string Title { get; set; } // nvarchar(50)
        [Column(name : "Owner", TypeName = "int")]
        [Required(ErrorMessage = "Owner is required")]
        [Display(Name = "Owner")]
        [Description("Employee who controls the document.  Foreign key to Employee.BusinessEntityID")]
        public int? Owner { get; set; } // int
        [Column(name : "FolderFlag", TypeName = "bit")]
        [Required(ErrorMessage = "Folder Flag is required")]
        [Display(Name = "Folder Flag")]
        [Description("0 = This is a folder, 1 = This is a document.")]
        [NotMapped]
        public bool? FolderFlag { get; set; } // bit
        [Column(name : "FileName")]
        [MaxLength(400)]
        [StringLength(400)]
        [Required(ErrorMessage = "File Name is required")]
        [Display(Name = "File Name")]
        [Description("File name of the document")]
        public string FileName { get; set; } // nvarchar(400)
        [Column(name : "FileExtension")]
        [MaxLength(8)]
        [StringLength(8)]
        [Required(ErrorMessage = "File Extension is required")]
        [Display(Name = "File Extension")]
        [Description("File extension indicating the document type. For example, .doc or .txt.")]
        public string FileExtension { get; set; } // nvarchar(8)
        [Column(name : "Revision", TypeName = "nchar")]
        [MaxLength(5)]
        [StringLength(5)]
        [Required(ErrorMessage = "Revision is required")]
        [Display(Name = "Revision")]
        [Description("Revision number of the document. ")]
        public string Revision { get; set; } // nchar(5)
        [Column(name : "ChangeNumber", TypeName = "int")]
        [Required(ErrorMessage = "Change Number is required")]
        [Display(Name = "Change Number")]
        [Description("Engineering change approval number.")]
        public int? ChangeNumber { get; set; } // int
        [Column(name : "Status")]
        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        [Description("1 = Pending approval, 2 = Approved, 3 = Obsolete")]
        public byte? Status { get; set; } // tinyint
        [Column(name : "DocumentSummary")]
        [MaxLength]
        [Display(Name = "Document Summary")]
        [Description("Document abstract.")]
        public string DocumentSummary { get; set; } // nvarchar(max)
        [Column(name : "Document")]
        [MaxLength]
        [Display(Name = "Document")]
        [Description("Complete document.")]
        public byte[] CompleteDocument { get; set; } // varbinary(max)
        [Column(name : "rowguid")]
        [Required(ErrorMessage = "rowguid is required")]
        [Display(Name = "rowguid")]
        [Description("ROWGUIDCOL number uniquely identifying the record. Required for FileStream.")]
        public Guid? rowguid { get; set; } // uniqueidentifier
        [Column(name : "ModifiedDate")]
        [Required(ErrorMessage = "Modified Date is required")]
        [Display(Name = "Modified Date")]
        [Description("Date and time the record was last updated.")]
        public DateTime? ModifiedDate { get; set; } // datetime

        // Production.Document.Owner -> HumanResources.Employee.BusinessEntityID (FK_Document_Employee_Owner)
        [ForeignKey("BusinessEntityID")]
        public Employee Employee { get; set; }
        // Production.ProductDocument.DocumentNode -> Production.Document.DocumentNode (FK_ProductDocument_Document_DocumentNode)
        public IEnumerable<ProductDocument> ProductDocuments { get; set; }
    }
}
