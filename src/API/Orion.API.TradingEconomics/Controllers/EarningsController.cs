// using Microsoft.AspNetCore.Mvc;
// using Orion.DataAccess.Postgres.Repositories;
//
//
// namespace Orion.API.TradingEconomics.Controllers;
// [ApiController]
// [Route("api/[controller]")]
// public class EarningsController : ControllerBase
// {
//         // GET
//       /// <summary>
//         /// get earnings calendar
//         /// </summary>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarnings()
//         {
//             return await HttpRequesterClass.HttpRequester("/earnings");
//         }
//
//         /// <summary>
//         /// Filter earnings by date
//         /// </summary>
//         /// <param name="startDate">Start date if needed</param>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarningsByDate(DateTime? startDate)
//         {
//             return await HttpRequesterClass.HttpRequester($"/earnings?d1={startDate.Value.ToString("yyyy-MM-dd")}");
//         }
//
//
//         /// <summary>
//         /// Get earnings by market, and start date
//         /// </summary>
//         /// <param name="symbol">symbol</param>
//         /// <param name="startDate">Start date</param>
//         /// <param name="endDate">End date</param>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarningsByMarketDate(string symbol, DateTime? startDate = null)
//         {
//
//             return await HttpRequesterClass.HttpRequester($"/earnings/symbol/{symbol}?d1={startDate.Value.ToString("yyyy-MM-dd")}");
//         }
//
//         /// <summary>
//         /// Get earnings by market, start and end date
//         /// </summary>
//         /// <param name="symbol">symbol</param>
//         /// <param name="startDate">Start date</param>
//         /// <param name="endDate">End date</param>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarningsMarketBetweenDates(string symbol, DateTime? startDate = null, DateTime? endDate = null)
//         {
//
//             return await HttpRequesterClass.HttpRequester($"/earnings/symbol/{symbol}?d1={startDate.Value.ToString("yyyy-MM-dd")}&d2={endDate.Value.ToString("yyyy-MM-dd")}");
//         }
//
//         /// <summary>
//         /// Get earnings by country
//         /// </summary>
//         /// <param name="country">symbol</param>
//         /// <param name="startDate">Start date</param>
//         /// <param name="endDate">End date</param>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarningsByCountry(string country)
//         {
//
//             return await HttpRequesterClass.HttpRequester($"/earnings/country/{country}");
//         }
//
//         /// <summary>
//         /// Get earnings by type
//         /// </summary>
//         /// <param name="type">symbol</param>
//         /// <param name="startDate">Start date</param>
//         /// <param name="endDate">End date</param>
//         /// <returns>A task that will be resolved in a string with the request result</returns>
//         public static async Task<string> GetEarningsByType(string type)
//         {
//
//             return await HttpRequesterClass.HttpRequester($"/earnings?type={type}");
//         }
// }