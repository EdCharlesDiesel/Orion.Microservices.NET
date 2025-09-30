namespace Orion.DataAccess.Postgres.AllFeatures;

public interface IFeatureManager
{
    bool CustomerSatisfaction { get; set; }
    bool Search { get; set; }
    bool SearchByBirthBusinessProvince { get; set; }
    
}