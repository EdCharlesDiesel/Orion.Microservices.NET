using Microsoft.AspNetCore.Mvc;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.Admin.Areas.API
{
    [Route("api/[controller]")]
    public class BusinessOwnerController(IBusinessOwnerService service) : ControllerBase
    {
        // GET: api/BusinessOwner
        [HttpGet]
        public IEnumerable<BusinessOwner> Get()
        {
            return service.GetBusinessOwners();
        }

        // GET: api/BusinessOwner/5
        [HttpGet("{id:int}", Name = "Get")]
        public BusinessOwner Get(int id)
        {
            return service.GetBusinessOwnerById(id);
        }
    }

    public interface IBusinessOwnerService
    {
        IEnumerable<BusinessOwner> GetBusinessOwners();
        BusinessOwner GetBusinessOwnerById(int id);
        object Search(string first, string last);
        void Save(BusinessOwner businessOwner);
        void DeleteBusinessOwnerById(int itemId);
        IList<BusinessOwner>? Search(string modelFirstName, string modelLastName, string modelBirthProvince, string modelBusinessProvince);
    }
}
