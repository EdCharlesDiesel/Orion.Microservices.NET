using Microsoft.AspNetCore.Mvc;

namespace Orion.WebApps.TradingEconomics.Controllers;

public class ComtradeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}