﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orion.DataAccess.Postgres.Data;
using Orion.DataAccess.Postgres.Entities;
using Orion.DataAccess.Postgres.Entities.Common;

namespace Orion.Admin.Controllers
{
    public class FeatureController : Controller
    {
        private OrionDbContext _Context;

        public FeatureController(OrionDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "Argument cannot be null.");
            }
            _Context = context;
        }

        // GET: Feature
        public ActionResult Index()
        {
            return View(_Context.Features.ToList());
        }

        // GET: Feature/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Feature feature = _Context.Features.Find(id);

            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // GET: Feature/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(include: "Id,Name,IsEnabled,Username")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                _Context.Features.Add(feature);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feature);
        }

        // GET: Feature/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var feature = _Context.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // POST: Feature/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(include: "Id,Name,IsEnabled,Username")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                _Context.Entry(feature).State = EntityState.Modified;
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        // GET: Feature/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Feature feature = _Context.Features.Find(id);
            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // POST: Feature/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feature feature = _Context.Features.Find(id);
            _Context.Features.Remove(feature);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
