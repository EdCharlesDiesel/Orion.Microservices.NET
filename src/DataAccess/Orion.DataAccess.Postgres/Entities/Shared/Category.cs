using System.ComponentModel.DataAnnotations;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities.Shared
{
    public sealed class Category: Entity<int>, ICategory
    {
        public void FullUpdate(ICategory o)
        {
            if (IsTransient())
            {
                Id = o.Id;
            }
            CategoryName = o.CategoryName;
            Description = o.Description;
            Picture = o.Picture;
        }  

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2"),MaxLength(30, ErrorMessage  = "Maximum length is 128")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only allowed letters")]
        public string CategoryName { get; private set; }

        public string Description { get; private set; }

        public byte[] Picture { get; private set; }

        public List<Product> Products { get; set; } = new();

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public Status Status { get; set; } = Status.Active;
    }       
}

