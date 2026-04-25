using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.WebApps.AnalysisDashboard.Models;
using Orion.WebApps.AnalysisDashboard.Services;
using System.Text.Json;

namespace Orion.WebApps.AnalysisDashboard.Pages;

public class IndexModel : PageModel
{
    private readonly ForexDataService _dataService;
    private readonly IWebHostEnvironment _environment;

    public IndexModel(ForexDataService dataService, IWebHostEnvironment environment)
    {
        _dataService = dataService;
        _environment = environment;
    }

    [BindProperty]
    public IFormFile? UploadedFile { get; set; }

    public ForexStatistics? Statistics { get; set; }
    public List<ForexCandle> RecentCandles { get; set; } = new();
    public string ChartDataJson { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
    {
        await LoadDataAsync(startDate, endDate);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (UploadedFile == null || UploadedFile.Length == 0)
        {
            TempData["Error"] = "Please select a file to upload.";
            return RedirectToPage();
        }

        if (!UploadedFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            TempData["Error"] = "Please upload a CSV file.";
            return RedirectToPage();
        }

        var result = await _dataService.ProcessFileAsync(UploadedFile);

        if (result.Success)
        {
            TempData["Success"] = result.Message;
        }
        else
        {
            TempData["Error"] = result.Message;
        }

        return RedirectToPage();
    }

    private async Task LoadDataAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var filteredData = _dataService.GetData(startDate, endDate);

        if (filteredData.Count > 0)
        {
            Statistics = _dataService.GetStatistics(filteredData);
            RecentCandles = filteredData.OrderByDescending(d => d.Timestamp).Take(1000).ToList();

            var chartData = _dataService.GetChartData(startDate, endDate, 5000);
            ChartDataJson = JsonSerializer.Serialize(chartData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }

    public JsonResult OnGetIndicatorData(string indicator, DateTime? startDate, DateTime? endDate)
    {
        var data = _dataService.GetIndicatorChartData(indicator, startDate, endDate);
        return new JsonResult(data);
    }
}