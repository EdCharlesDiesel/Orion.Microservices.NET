using System.ComponentModel.DataAnnotations;

namespace Orion.StockAnalyzer.Core.BaseClasses;

public class EntityKeyless
{

    [Required]
    public DateTime CreatedDate  { get; set; } = DateTime.UtcNow;
    [Required]
    public string CreateBy  { get; set; } = "System";
    public DateTime ModifiedDate  { get; set; }
    public string ModifiedBy  { get; set; } ="System";
}