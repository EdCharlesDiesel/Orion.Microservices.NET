using System.ComponentModel.DataAnnotations;

namespace Orion.API.HumanResources.Models
{
    public class InternalEmployeeDtoCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;           
    }
}
