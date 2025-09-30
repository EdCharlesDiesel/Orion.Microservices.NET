using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orion.Admin.Areas.API;
using Orion.Admin.Models;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;


namespace Orion.Admin.Controllers
{
    public class SecurityAdminController : Controller
    {
        private readonly UserManager<MasterUser> _userManager;
        private readonly IBusinessOwnerService _businessOwnerService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private OrionDbContext _context;
        private readonly ISubscriptionService _SubscriptionService;

        public SecurityAdminController(UserManager<MasterUser> userManager, 
            RoleManager<IdentityRole> roleManager, IBusinessOwnerService businessOwnerService,
            OrionDbContext dbContext, ISubscriptionService subscriptionService)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager), $"{nameof(roleManager)} is null.");
            }

            if (businessOwnerService == null)
            {
                throw new ArgumentNullException("businessOwnerService", "Argument cannot be null.");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager), $"{nameof(userManager)} is null.");
            }

            _userManager = userManager;
            _businessOwnerService = businessOwnerService;
            _roleManager = roleManager;
            _context = dbContext;
            _SubscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {
            var model = new SecurityAdminAction();

            model.BusinessOwners =
                AdaptToSelectListItems(_businessOwnerService.GetBusinessOwners());

            model.Roles = AdaptToSelectListItems(_roleManager.Roles.ToList());

            // FIXME This needs fixing
           // model.Users = AdaptToSelectListItems(_UserManager.Users.OrderBy(x => x.UserName).ToList());

            model.Permissions = new List<SelectListItem>();

            model.Permissions.Add(new SelectListItem
            {
                Value = SecurityConstants.PermissionNameEdit,
                Text = SecurityConstants.PermissionNameEdit
            });

            model.Permissions.Add(new SelectListItem
            {
                Value = SecurityConstants.PermissionNameView,
                Text = SecurityConstants.PermissionNameView
            });

            model.Claims = GetAllClaims();

            model.SubscriptionTypes = new List<SelectListItem>();

            model.SubscriptionTypes.Add(new SelectListItem
            {
                Value = SecurityConstants.SubscriptionTypeBasic,
                Text = SecurityConstants.SubscriptionTypeBasic
            });

            model.SubscriptionTypes.Add(new SelectListItem
            {
                Value = SecurityConstants.SubscriptionTypeUltimate,
                Text = SecurityConstants.SubscriptionTypeUltimate
            });

            return View(model);
        }

        private List<SelectListItem> AdaptToSelectListItems(IEnumerable<BusinessOwner> getBusinessOwners)
        {
            throw new NotImplementedException();
        }

        private List<ClaimViewModel> GetAllClaims()
        {

            // var users = (from temp in _Context.Users
            //              select temp);
            //
            // Dictionary<string, string> usersDictionary = new Dictionary<string, string>();
            //
            // foreach(var usr in users)
            // {
            //     usersDictionary.Add(usr.Id.ToString(), usr.UserName);
            // }
            //
            // Dictionary<string, string> businessOwnersDictionary = new Dictionary<string, string>();
            //
            // object _BusinessOwnerService;
            // foreach (var businessOwner in _BusinessOwnerService.GetBusinessOwners())
            // {
            //     businessOwnersDictionary.Add(businessOwner.Id.ToString(),
            //         String.Format("{0} {1}", businessOwner.FirstName, businessOwner.LastName)
            //         );
            // }
            //
            // var claims = (
            //     from temp in _Context.UserClaims
            //     select temp
            //     );
            //
            // List<ClaimViewModel> returnValues = new List<ClaimViewModel>();
            //
            // foreach (var claim in claims)
            // {
            //     var temp = new ClaimViewModel(claim.Id.ToString(), usersDictionary[claim.UserId.ToString()],
            //         claim.ClaimType, businessOwnersDictionary[claim.ClaimValue]);
            //
            //     returnValues.Add(temp);
            // }
            //
            // return returnValues;
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> AddToGroup(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.RoleId != null &&
                securityAdminAction.UserId != null)
            {
                var role = await _roleManager.FindByIdAsync(securityAdminAction.RoleId);
                var user = await _userManager.FindByIdAsync(securityAdminAction.UserId);

                await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromGroup(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.RoleId != null &&
                securityAdminAction.UserId != null)
            {
                var role = await _roleManager.FindByIdAsync(securityAdminAction.RoleId);
                var user = await _userManager.FindByIdAsync(securityAdminAction.UserId);

                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddSubscription(
            SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.UserId != null)
            {
                _SubscriptionService.AddSubscription(
                    GetSelectedUsername(securityAdminAction), 
                    securityAdminAction.SubscriptionType);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveSubscription(
            SecurityAdminAction securityAdminAction)
        {
            // if (securityAdminAction != null &&
            //     securityAdminAction.UserId != null)
            // {
            //     _SubscriptionService.RemoveSubscription(
            //         GetSelectedUsername(securityAdminAction));
            // }
            //
            // return RedirectToAction("Index");
            throw new NotImplementedException();
        }

        private string GetSelectedUsername(SecurityAdminAction securityAdminAction)
        {
            // var allUsers = _userManager.Users.ToList();
            //
            // var username = (from temp in allUsers
            //                 where temp.Id.ToString() == securityAdminAction.UserId
            //                 select temp).FirstOrDefault();
            //
            // if (username != null)
            // {
            //     return username.UserName;
            // }
            //
            // return securityAdminAction.UserId;
            throw new NotImplementedException();
        }

        private List<SelectListItem> AdaptToSelectListItems(List<IdentityRole> fromValues)
        {
            List<SelectListItem> toValues = new List<SelectListItem>();

            foreach (var fromValue in fromValues)
            {
                toValues.Add(new SelectListItem
                {
                    Value = fromValue.Id,
                    Text = fromValue.Name
                });
            }

            return toValues;
        }

        private List<SelectListItem> AdaptToSelectListItems(IList<BusinessOwner> fromValues)
        {
            // List<SelectListItem> toValues = new List<SelectListItem>();
            //
            // foreach (var fromValue in fromValues.OrderBy(x => x.LastName))
            // {
            //     toValues.Add(new SelectListItem
            //     {
            //         Value = fromValue.Id.ToString(),
            //         Text = String.Format("{0}, {1}", fromValue.LastName, fromValue.FirstName)
            //     });
            // }
            //
            // return toValues;
            throw new NotImplementedException();
        }

        private List<SelectListItem> AdaptToSelectListItems(List<IdentityUser> fromValues)
        {
            List<SelectListItem> toValues = new List<SelectListItem>();

            foreach (var fromValue in fromValues)
            {
                toValues.Add(new SelectListItem
                {
                    Value = fromValue.Id,
                    Text = fromValue.UserName
                });
            }

            return toValues;
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.BusinessOwnerId != null &&
                securityAdminAction.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(securityAdminAction.UserId);

                var claim = new Claim(
                    securityAdminAction.Permission,
                    securityAdminAction.BusinessOwnerId);

                var claims = await _userManager.GetClaimsAsync(user);

                /*
                var removeThisClaim = (
                    from temp in claims
                    where temp.Type == SecurityConstants.Claim_LevelOfAwesomeness
                    select temp
                    ).FirstOrDefault();

                if (removeThisClaim != null)
                {
                    await _UserManager.RemoveClaimAsync(user, removeThisClaim);
                }
                */

                await _userManager.AddClaimAsync(user, claim);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemovePermission(int id)
        {
            // var removeThis = (from temp in _Context.UserClaims
            //                   where temp.Id == id
            //                   select temp).FirstOrDefault();
            //
            // if (removeThis != null)
            // {
            //     _Context.UserClaims.Remove(removeThis);
            //     _Context.SaveChanges();
            // }
            //
            // return RedirectToAction("Index");

            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> RemovePermission(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.BusinessOwnerId != null &&
                securityAdminAction.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(securityAdminAction.UserId);

                var claims = await _userManager.GetClaimsAsync(user);

                var removeThisClaim = (
                    from temp in claims
                    where temp.Type == securityAdminAction.Permission
                    select temp
                    ).FirstOrDefault();

                if (removeThisClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, removeThisClaim);
                }
            }

            return RedirectToAction("Index");
        }
    }

    public interface ISubscriptionService
    {
        void AddSubscription(string getSelectedUsername, string subscriptionType);
        string GetSubscriptionType(string username);
    }
}