using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.TradingEconomics;
using Orion.WebApps.TradingEconomics.Helper;

namespace Orion.WebApps.TradingEconomics.Controllers;

public class CalendarController(
    IHttpClientFactory httpClientFactory,
    IOptions<ApiSettings> apiOptions,
    ICalendarServices iCalendarServices)
    : Controller
{
    private readonly ApiSettings _apiSettings = apiOptions.Value;

    // In-memory store (simulate a database)
    private static List<OrionCalendarEvent> _calendarEvents = new();

    // GET: /Calendar
    public async Task<List<OrionCalendarEvent>> Index()
    {
        //TODO: Fix later
        throw new NotImplementedException();
        // var client = httpClientFactory.CreateClient();
        //
        // try
        // {
        //     var events = await client.GetFromJsonAsync<List<OrionCalendarEvent>>(_apiSettings.CalendarApiUrl);
        //     _calendarEvents = events ?? new List<OrionCalendarEvent>();
        //     if (events != null) await iCalendarServices.Create(events);
        //
        //     return View(_calendarEvents);
        // }
        // catch (HttpRequestException ex)
        // {
        //     ViewBag.Error = "API error: " + ex.Message;
        //     return View("Error");
        // }
    }

    // GET: /Calendar/Details/{id}
    public async Task<IActionResult> Details(Guid id)
    {
        var client = httpClientFactory.CreateClient();
        var events = await client.GetFromJsonAsync<List<OrionCalendarEvent>>(_apiSettings.CalendarApiUrl);
        var item = events?.FirstOrDefault(c => c.Id == id);

        if (item == null)
            return NotFound();

        return View(item);
    }

    // GET: /Calendar/Create
    public Task<IActionResult> Create()
    {
        return Task.FromResult<IActionResult>(View());
    }

    // POST: /Calendar/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrionCalendarEvent calendar)
    {
        //TODO: Fix later
        throw new NotImplementedException();
        // if (ModelState.IsValid)
        // {
        //     calendar.Id = Guid.NewGuid();
        //     _calendarEvents.Add(calendar);
        //
        //     // Simulate async I/O operation
        //     await Task.CompletedTask;
        //
        //     return RedirectToAction(nameof(Index));
        // }
        //
        // return View(calendar);
    }

    // GET: /Calendar/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        //TODO: Fix later
        throw new NotImplementedException();
        // var item = await Task.FromResult(_calendarEvents.FirstOrDefault(c => c.Id == id));
        // if (item == null)
        //     return NotFound();
        //
        // return View(item);
    }

    // POST: /Calendar/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TradingEconomicsCalendar updated)
    {
        var item = _calendarEvents.FirstOrDefault(c => c.Id == id);
        if (item == null)
            return NotFound();

        if (ModelState.IsValid)
        {
           
            item.Date = updated.Date;
         

            await Task.CompletedTask;

            return RedirectToAction(nameof(Index));
        }

        return View(updated);
    }

    // GET: /Calendar/Delete/{id}
    public async Task<IActionResult> Delete(Guid id)
    {
        //TODO: Fix later
        throw new NotImplementedException();
        // var item = await Task.FromResult(_calendarEvents.FirstOrDefault(c => c.Id == id));
        // if (item == null)
        //     return NotFound();
        //
        // return View(item);
    }

    // POST: /Calendar/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var item = _calendarEvents.FirstOrDefault(c => c.Id == id);
        if (item != null)
        {
            _calendarEvents.Remove(item);
        }

        await Task.CompletedTask;
        return RedirectToAction(nameof(Index));
    }
}

public interface ICalendarServices
{
}