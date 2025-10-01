using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Orion.DataAccess.Postgres.Aggregates;
using Orion.DataAccess.Postgres.Tools;
using Orion.Domain.Enums;

namespace Orion.DataAccess.Postgres.Entities.Shared
{
    [Table("Feature", Schema = "Shared")]
    public class Feature : Entity<int>, IFeature
    {
        // public void FullUpdate(IFeature o)
        // {
        //    if (IsTransient())
        //    {
        //        Id = o.;
        //    }
        //    FeatureName = o.FeatureName;
        //    IsEnabled = o.IsEnabled;
        //    Username = o.Username;
        // }

        public void FullUpdate(IFeature o)
        {
            throw new NotImplementedException();
        }

        [Display(Name = "Feature Name")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FeatureName { get; set; }

        [Display(Name = "Is Enabled")]
        public bool IsEnabled { get; set; }

        [Display(Name = "For Username")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Username { get; set; }


       private DateTime _createDate = DateTime.Now;
        
        public DateTime CreateDate { get => _createDate; set => _createDate = value; }

        public DateTime? UpdateDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        private Status _status = Status.Active;
        
        public Status Status { get => _status; set => _status = value; }

       
    }
}
