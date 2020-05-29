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
    public class NeighbourhoodsController : Controller
    {
        private readonly DBHospitalContext _context;

        public NeighbourhoodsController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Neighbourhoods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Neighbourhoods.ToListAsync());
        }

        // GET: Neighbourhoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighbourhoods = await _context.Neighbourhoods
                .FirstOrDefaultAsync(m => m.NeighbourhoodId == id);
            if (neighbourhoods == null)
            {
                return NotFound();
            }

            return View(neighbourhoods);
        }

        // GET: Neighbourhoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Neighbourhoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NeighbourhoodId,NeighbourhoodNaming")] Neighbourhoods neighbourhoods)
        {
            if (ModelState.IsValid)
            {
                if (_context.Neighbourhoods.Where(pr => pr.NeighbourhoodNaming == neighbourhoods.NeighbourhoodNaming).Count() == 0)

                {
                    _context.Add(neighbourhoods);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
            }
               
                return View(neighbourhoods);
        }


        // GET: Neighbourhoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighbourhoods = await _context.Neighbourhoods.FindAsync(id);
            if (neighbourhoods == null)
            {
                return NotFound();
            }
            return View(neighbourhoods);
        }

        // POST: Neighbourhoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NeighbourhoodId,NeighbourhoodNaming")] Neighbourhoods neighbourhoods)
        {
            if (id != neighbourhoods.NeighbourhoodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Neighbourhoods.Where(pr => pr.NeighbourhoodNaming == neighbourhoods.NeighbourhoodNaming).Count() == 0)

                {
                    try
                    {
                        _context.Update(neighbourhoods);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NeighbourhoodsExists(neighbourhoods.NeighbourhoodId))
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
            return View(neighbourhoods);
        }

        // GET: Neighbourhoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neighbourhoods = await _context.Neighbourhoods
                .FirstOrDefaultAsync(m => m.NeighbourhoodId == id);
            if (neighbourhoods == null)
            {
                return NotFound();
            }

            return View(neighbourhoods);
        }

        // POST: Neighbourhoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var neighbourhoods = await _context.Neighbourhoods.FindAsync(id);
            _context.Neighbourhoods.Remove(neighbourhoods);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NeighbourhoodsExists(int id)
        {
            return _context.Neighbourhoods.Any(e => e.NeighbourhoodId == id);
        }
    }
}
