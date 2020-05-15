using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EsportMVC;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;


namespace EsportMVC.Controllers
{
    
    public class CountriesController : Controller
    {
        private readonly EsportDBContext _context;

        public CountriesController(EsportDBContext context)
        {
            _context = context;
        }

        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Countries.ToListAsync());
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            //return View(country);
            return RedirectToAction("Index", "Organisations", new { id = country.Id, name = country.Name });
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Country country)
        {
            if (ModelState.IsValid)
            {
                var c = (from coun in _context.Countries
                         where coun.Name.Contains(country.Name)
                         select coun).ToList();

                if (c.Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                if(!Valid.IsAllLetters(country.Name))
                    return RedirectToAction(nameof(Index));


                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
             return View(country);
            
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                if (!Valid.IsAllLetters(worksheet.Name))
                                    continue;
                                Country newcoun;
                                var c = (from coun in _context.Countries
                                         where coun.Name.Contains(worksheet.Name)
                                         select coun).ToList();
                                
                                if (c.Count > 0)
                                {
                                    newcoun = c[0];
                                }
                                
                                else
                                {
                                    newcoun = new Country();
                                    newcoun.Name = worksheet.Name;
                                    
                                    _context.Countries.Add(newcoun);
                                }
                                                   
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Organisation organisation = new Organisation();
                                        organisation.Name = row.Cell(1).Value.ToString();
                                         


                                        organisation.CreationDate = DateTime.Parse(row.Cell(2).Value.ToString());
                                        if(!Valid.Datecheck(organisation.CreationDate))
                                        {
                                            continue;
                                        }
                                        //int Day = Int.Parse(row.Cell(2).Value.ToString());
                                        //int Mounth = Int.Parse(row.Cell(3).Value.ToString());
                                        //int Year = Int.Parse(row.Cell(4).Value.ToString());
                                        //organisation.CreationDate = new DateTime(Year, Mounth, Day);
                                        organisation.Country = newcoun;
                                        var dcp = (from org in _context.Organisations
                                                 where org.Name.Contains(organisation.Name)
                                                 select org).ToList();
                                        if(dcp.Count > 0)
                                        {
                                            continue;
                                        }
                                        _context.Organisations.Add(organisation);
                                        
                                    }
                                    catch (Exception e)
                                    {
                                        

                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var countries = _context.Countries.Include("Organisations").ToList();
                
                foreach (var c in countries)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name);

                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Дата";
                
                   
                    worksheet.Row(1).Style.Font.Bold = true;
                    var organisations = c.Organisations.ToList();

                    
                    for (int i = 0; i < organisations.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = organisations[i].Name;

                        worksheet.Cell(i + 2, 2).Value = organisations[i].CreationDate;
                    }

                     
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"not_a_library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
