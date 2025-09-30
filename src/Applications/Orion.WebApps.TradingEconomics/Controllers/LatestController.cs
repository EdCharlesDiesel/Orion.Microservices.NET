using Microsoft.AspNetCore.Mvc;
using Orion.WebApps.TradingEconomics.Helper;

namespace Orion.WebApps.TradingEconomics.Controllers
{
    [Route("[controller]")]
    public class LatestController : Controller
    {
        // GET: /Latest
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var latestUpdates = await GetLatestUpdates();
            ViewBag.LatestUpdates = latestUpdates;
            return View("Index");
        }

        // GET: /Latest/ByDate/2025-07-01
        [HttpGet("ByDate/{date}")]
        public async Task<IActionResult> ByDate(DateTime date)
        {
            var updatesByDate = await GetLatestUpdatesByDate(date);
            ViewBag.UpdatesByDate = updatesByDate;
            ViewBag.RequestedDate = date;
            return View("ByDate");
        }

        // Static method to get updates (simulate external call)
        private static async Task<string> GetLatestUpdates()
        {
            return await HttpRequesterClass.HttpRequester("/updates");
        }

        private static async Task<string> GetLatestUpdatesByDate(DateTime startDate)
        {
            return await HttpRequesterClass.HttpRequester($"/updates/{startDate:yyyy-MM-dd}");
        }
    }
}
    
