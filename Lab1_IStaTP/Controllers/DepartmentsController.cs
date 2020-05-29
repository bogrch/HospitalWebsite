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
    public class DepartmentsController : Controller
    {
        private readonly DBHospitalContext _context;

        public DepartmentsController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (departments == null)
            {
                return NotFound();
            }

            // return View(departments);
            return RedirectToAction("Index", "Employees", new { id = departments.DepartmentId, name = departments.DepartmentNaming });
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DepartmentNaming,ChiefOfDepartment,QuantityOfPlaces")] Departments departments)
        {
            if (ModelState.IsValid)
            {
                if (_context.Departments.Where(pr => pr.DepartmentNaming == departments.DepartmentNaming).Count() == 0)
                {
                    _context.Add(departments);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
            }
            return View(departments);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments.FindAsync(id);
            if (departments == null)
            {
                return NotFound();
            }
            return View(departments);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentNaming,ChiefOfDepartment,QuantityOfPlaces")] Departments departments)
        {
            if (id != departments.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Departments.Where(pr => pr.DepartmentNaming == departments.DepartmentNaming).Count() == 0)
                {
                    try
                    {
                        _context.Update(departments);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DepartmentsExists(departments.DepartmentId))
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
            return View(departments);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departments = await _context.Departments
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (departments == null)
            {
                return NotFound();
            }

            return View(departments);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departments = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(departments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentsExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
