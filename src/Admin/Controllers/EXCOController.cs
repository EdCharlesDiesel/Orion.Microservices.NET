using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Orion.Admin.Models;

namespace Orion.Admin.Controllers
{

    public class ExcoController(ILogger<ExcoController> logger) : Controller
    {
        private readonly ILogger<ExcoController> _logger = logger;

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
            // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            throw new NotImplementedException();
        }
    }
}