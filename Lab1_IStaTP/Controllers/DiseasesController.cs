using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_IStaTP;

namespace Lab1_IStaTP.Controllers
{
    public class DiseasesController : Controller
    {
        private readonly DBHospitalContext _context;

        public DiseasesController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Diseases
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Categories");
            // finding disease by category
            ViewBag.CategoryId = id;
            ViewBag.CategoryNaming = name;
            var diseasesByCategory = _context.Diseases.Where(d => d.CategoryId == id).Include(d => d.Category);
            return View(await diseasesByCategory.ToListAsync());
        }

        // GET: Diseases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diseases = await _context.Diseases
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DiseaseId == id);
            if (diseases == null)
            {
                return NotFound();
            }

            return View(diseases);
        }

        // GET: Diseases/Create
        public IActionResult Create(int categoryID)
        {
            ViewBag.CategoryID = categoryID;
            ViewBag.CategoryNaming = _context.Categories.Where(c => c.CategoryId == categoryID).FirstOrDefault().CategoryNaming;
            return View();
        }

        // POST: Diseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryID,[Bind("DiseaseId,CategoryId,DiseaseNaming")] Diseases diseases)
        {
            diseases.CategoryId = categoryID;

            if (ModelState.IsValid)
            {
                if (_context.Diseases.Where(pr => pr.DiseaseNaming == diseases.DiseaseNaming).Count() == 0)
                {
                    _context.Add(diseases);
                    await _context.SaveChangesAsync();
                    //    return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "Diseases", new { id = categoryID, name = _context.Categories.Where(c => c.CategoryId == categoryID).FirstOrDefault().CategoryNaming });
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");

                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", diseases.CategoryId);
            //return View(diseases);
            return RedirectToAction("Index", "Diseases", new { id = categoryID, name = _context.Categories.Where(c => c.CategoryId == categoryID).FirstOrDefault().CategoryNaming });

        }

        // GET: Diseases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diseases = await _context.Diseases.FindAsync(id);
            if (diseases == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", diseases.CategoryId);
            return View(diseases);
        }

        // POST: Diseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiseaseId,CategoryId,DiseaseNaming")] Diseases diseases)
        {
            if (id != diseases.DiseaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Diseases.Where(pr => pr.DiseaseNaming == diseases.DiseaseNaming).Count() == 0)
                {
                    try
                    {
                        _context.Update(diseases);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DiseasesExists(diseases.DiseaseId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
                
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", diseases.CategoryId);
            return View(diseases);
        }

        // GET: Diseases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diseases = await _context.Diseases
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DiseaseId == id);
            if (diseases == null)
            {
                return NotFound();
            }

            return View(diseases);
        }

        // POST: Diseases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diseases = await _context.Diseases.FindAsync(id);
            _context.Diseases.Remove(diseases);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiseasesExists(int id)
        {
            return _context.Diseases.Any(e => e.DiseaseId == id);
        }
    }
}
