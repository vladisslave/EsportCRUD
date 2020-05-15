using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EsportMVC;

namespace EsportMVC.Controllers
{
    public class OrganisationsController : Controller
    {
        private readonly EsportDBContext _context;

        public OrganisationsController(EsportDBContext context)
        {
            _context = context;
        }

        // GET: Organisations
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Countries", "Index");
            //znaxodjennia орги za krajinoju
            ViewBag.CountryId = id;
            ViewBag.CountryName = name;
            var organisationsByCountry = _context.Organisations.Where(b => b.CountryId == id).Include(b => b.Country);


            return View(await organisationsByCountry.ToListAsync());
        }

        // GET: Organisations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisation = await _context.Organisations
                .Include(o => o.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organisation == null)
            {
                return NotFound();
            }

            // return View(organisation);
            return RedirectToAction("Index", "Departments", new { id = organisation.Id, name = organisation.Name });
        }

        // GET: Organisations/Create
        public IActionResult Create(int countryId)
        {
            // ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewBag.CountryId = countryId;
            ViewBag.CountryName = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name;
            return View();
        }

        // POST: Organisations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int countryId, [Bind("Id,Name,CreationDate")] Organisation organisation)
        {
            organisation.CountryId = countryId;
            if (ModelState.IsValid)
            {
                var c = (from org in _context.Organisations
                         where org.Name.Contains(organisation.Name)
                         select org).ToList();

                if (c.Count > 0)
                {
                    return RedirectToAction("Index", "Organisations", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name });
                }
                if (!Valid.Datecheck(organisation.CreationDate))
                {
                    return RedirectToAction("Index", "Organisations", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name });
                }

                _context.Add(organisation);
                await _context.SaveChangesAsync();
                // return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Organisations", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name });
            }
            // ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", organisation.CountryId);
            //   return View(organisation);
            return RedirectToAction("Index", "Organisations", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name });
        }

        // GET: Organisations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisation = await _context.Organisations.FindAsync(id);
            if (organisation == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", organisation.CountryId);
            return View(organisation);
        }

        // POST: Organisations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId,CreationDate")] Organisation organisation)
        {
            if (id != organisation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organisation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganisationExists(organisation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Countries");
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", organisation.CountryId);
            return View(organisation);
        }

        // GET: Organisations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisation = await _context.Organisations
                .Include(o => o.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organisation == null)
            {
                return NotFound();
            }

            return View(organisation);
        }

        // POST: Organisations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organisation = await _context.Organisations.FindAsync(id);
            _context.Organisations.Remove(organisation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Countries");
           // return RedirectToAction("organisation");
        }

        private bool OrganisationExists(int id)
        {
            return _context.Organisations.Any(e => e.Id == id);
        }
    }
}
