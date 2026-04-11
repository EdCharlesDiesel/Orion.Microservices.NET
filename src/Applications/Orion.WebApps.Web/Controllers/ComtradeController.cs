using Microsoft.AspNetCore.Mvc;

namespace Orion.WebApps.Web.Controllers;

public class ComtradeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}