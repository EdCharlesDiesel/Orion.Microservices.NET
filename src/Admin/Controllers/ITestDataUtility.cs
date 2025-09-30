namespace Orion.Admin.Controllers
{
    public interface ITestDataUtility
    {
        Task CreateBusinessOwnerTestData();
        Task VerifyDatabaseIsPopulated();
    }

}
