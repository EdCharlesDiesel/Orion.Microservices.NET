﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.ApiServices;
using Orion.Shared;

namespace Orion.Controllers;

[Authorize]
[ServiceFilter(typeof(EnsureAccessTokenFilter))]
public class ConferenceController : Controller
{
    private readonly IConferenceApiService _ApiService;

    public ConferenceController(IConferenceApiService service)
    {
        _ApiService = service;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Title = "Organizer - Conference Overview";
        return View(await _ApiService.GetAll());
    }

    public IActionResult Add()
    {
        ViewBag.Title = "Organizer - Add Conference";
        return View(new ConferenceModel());
    }

    [HttpPost]
    public async Task<IActionResult> Add(ConferenceModel model)
    {
        if (ModelState.IsValid)
            await _ApiService.Add(model);

        return RedirectToAction("Index");
    }
}
