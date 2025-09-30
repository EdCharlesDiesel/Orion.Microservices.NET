
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orion.DataAccess.Postgres.Entities
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int DurationInMinutes { get; set; } // course length

        [Required]
        public bool IsMandatory { get; set; } = false; // obligatory courses

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true; // soft delete / active flag
    }
}
