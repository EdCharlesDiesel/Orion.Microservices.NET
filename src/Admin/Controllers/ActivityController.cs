using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Models;

namespace Orion.Admin.Controllers
{

    // ToDo: Add commands and event handlers
    public class ActivityController(ILogger<ActivityController> logger) : Controller
    {
        private readonly ILogger<ActivityController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            throw new NotImplementedException();
            // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}