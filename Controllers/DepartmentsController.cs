﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EsportMVC;

namespace EsportMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly EsportDBContext _context;

        public DepartmentsController(EsportDBContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Organisations", "Index");
            //znaxodjennia depy za orgoyuu
            ViewBag.OrganisationId = id;
            ViewBag.OrganisationName = name;
            var departmentsByOrganisation = _context.Departments.Where(b => b.OrganisationId == id).Include(b => b.Organisation);


            return View(await departmentsByOrganisation.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Organisation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create(int organisationId)
        {
            // ViewData["OrganisationId"] = new SelectList(_context.Organisations, "Id", "Name");
            ViewBag.OrganisationId = organisationId;
            ViewBag.OrganisationName = _context.Organisations.Where(c => c.Id == organisationId).FirstOrDefault().Name;
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int organisationId, [Bind("Id,Name")] Department department)
        {
            department.OrganisationId = organisationId;
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Departments", new { id = organisationId, name = _context.Organisations.Where(c => c.Id == organisationId).FirstOrDefault().Name });
            }
            // ViewData["OrganisationId"] = new SelectList(_context.Organisations, "Id", "Name", department.OrganisationId);
            // return View(department);
            return RedirectToAction("Index", "Departments", new { id = organisationId, name = _context.Organisations.Where(c => c.Id == organisationId).FirstOrDefault().Name });

        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["OrganisationId"] = new SelectList(_context.Organisations, "Id", "Name", department.OrganisationId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganisationId,Name")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganisationId"] = new SelectList(_context.Organisations, "Id", "Name", department.OrganisationId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Organisation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
