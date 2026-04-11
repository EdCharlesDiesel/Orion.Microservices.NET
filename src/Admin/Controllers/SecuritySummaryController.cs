using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Orion.Admin.Controllers
{
    public class SecuritySummaryController : Controller
    {
        public IActionResult Index()
        {
            var principal = User;

            var identity = User.Identity;

            var claimsIdentityInstance = identity as ClaimsIdentity;

            if (claimsIdentityInstance == null)
            {
                return View(new List<Claim>());
            }

            return View(claimsIdentityInstance.Claims.ToList());
        }
    }
}