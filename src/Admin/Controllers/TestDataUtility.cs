using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Orion.Admin.Areas.API;
using Orion.Admin.TestData;
using Orion.DataAccess.Postgres.AllFeatures;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;

namespace Orion.Admin.Controllers
{
    public class TestDataUtility : ITestDataUtility
    {
        private IBusinessOwnerService _service;
        private OrionDbContext _dbContext;
        private UserManager<IdentityUser> _UserManager;
        private RoleManager<IdentityRole> _RoleManager;

        public TestDataUtility(IBusinessOwnerService service, OrionDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            if (service == null)
                throw new ArgumentNullException("service", "service is null.");

            _service = service;

            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext", "Argument cannot be null.");
            }

            _dbContext = dbContext;

            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public async Task CreateBusinessOwnerTestData()
        {
            var xml = TestDataResource.zaBusinessOwnersXml;

            List<BusinessOwner> allBusinessOwners = PopulateBusinessOwnersFromXml(xml);

            DeleteAll();

            allBusinessOwners.ForEach(x => _service.Save(x));

            await InitializeSecurity();
        }

        public async Task VerifyDatabaseIsPopulated()
        {
            // _dbContext.Database.GetHashCode();
            //
            // var businessOwners = _service.GetBusinessOwners();
            //
            // if (businessOwners == null || businessOwners.Count == 0)
            // {
            //     await CreateBusinessOwnerTestData();
            // }
            
            throw new NotImplementedException();
        }

        private List<BusinessOwner> PopulateBusinessOwnersFromXml(string xml)
        {
            var returnValue = new List<BusinessOwner>();

            var root = XElement.Parse(xml);

            var businessOwners = root.ElementsByLocalName("businessOwner");

            BusinessOwner? groverCleveland = null;

            foreach (var fromElement in businessOwners)
            {
                var currentBusinessOwner = GetBusinessOwnerFromXml(fromElement);

                if (currentBusinessOwner.LastName == "Cleveland")
                {
                    // grover cleveland had two non-consecutive terms
                    // only create one record for grover 
                    // with two terms
                    if (groverCleveland == null)
                    {
                        groverCleveland = currentBusinessOwner;
                        returnValue.Add(currentBusinessOwner);
                    }
                    //FIXME Please fix this
                    //  groverCleveland.Terms.Add(currentBusinessOwner.Terms[0]);
                }
                else
                {
                    returnValue.Add(currentBusinessOwner);
                }
            }

            return returnValue;
        }

        private BusinessOwner GetBusinessOwnerFromXml(XElement fromValue)
        {
            // BusinessOwner toValue = new BusinessOwner();
            //
            // toValue.BirthCity = fromValue.AttributeValue("birthcity");
            // toValue.BirthProvince = fromValue.AttributeValue("birthstate");
            // toValue.BirthDate = SafeToDateTime(fromValue.AttributeValue("birthdate"));
            //
            // toValue.BusinessCity = fromValue.AttributeValue("deathcity");
            // toValue.BusinessProvince = fromValue.AttributeValue("deathstate");
            // toValue.BusinessDate = SafeToDateTime(fromValue.AttributeValue("deathdate"));
            //
            // toValue.FirstName = fromValue.AttributeValue("firstname");
            // toValue.LastName = fromValue.AttributeValue("lastname");
            //
            // toValue.ImageFilename = fromValue.AttributeValue("image-filename");
            //
            // toValue.AddTerm(
            //     "BusinessOwner",
            //     SafeToDateTime(fromValue.AttributeValue("start")),
            //     SafeToDateTime(fromValue.AttributeValue("end")),
            //     SafeToInt32(fromValue.AttributeValue("id"))
            //     );
            //
            // return toValue;
            throw new NotImplementedException();
        }

        private DateTime SafeToDateTime(string fromValue)
        {
            DateTime temp;

            if (DateTime.TryParse(fromValue, out temp))
            {
                return temp;
            }

            return default(DateTime);
        }

        private int SafeToInt32(string fromValue)
        {
            int temp;

            if (Int32.TryParse(fromValue, out temp))
            {
                return temp;
            }

            return default(int);
        }

        private void DeleteAll()
        {
            // var allBusinessOwners = _service.GetBusinessOwners();
            //
            // foreach (var item in allBusinessOwners)
            // {
            //     _service.DeleteBusinessOwnerById(item.Id);
            // }
            throw new NotImplementedException();
        }
        private async Task InitializeSecurity()
        {
            await DeleteAllRoles();
            await DeleteAllUsers();

            // create the roles
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleNameAdmin));
            await _RoleManager.CreateAsync(new IdentityRole(SecurityConstants.RoleNameUser));

            // create users
            var admin = await CreateUser(SecurityConstants.UsernameAdmin);
            var user1 = await CreateUser(SecurityConstants.UsernameUser1);
            var user2 = await CreateUser(SecurityConstants.UsernameUser2);
            var user3 = await CreateUser(SecurityConstants.UsernameSubscriber1);
            var user4 = await CreateUser(SecurityConstants.UsernameSubscriber2);
        }

        private async Task<IdentityUser> CreateUser(string username)
        {
            var user = new IdentityUser();

            user.UserName = username;
            user.Email = username;
            user.EmailConfirmed = true;

            var result = await _UserManager.CreateAsync(user, SecurityConstants.DefaultPassword);

            if (result.Succeeded == false)
            {
                throw new InvalidOperationException("Error while creating user." + Environment.NewLine + result.Errors);
            }

            Console.WriteLine();

            return user;
        }

        private async Task DeleteAllUsers()
        {
            var users = _UserManager.Users.ToList();

            foreach (var deleteThisUser in users)
            {
                await _UserManager.DeleteAsync(deleteThisUser);
            }
        }

        private async Task DeleteAllRoles()
        {
            var roles = _RoleManager.Roles.ToList();

            foreach (var deleteThis in roles)
            {
                await _RoleManager.DeleteAsync(deleteThis);
            }
        }
    }

}
