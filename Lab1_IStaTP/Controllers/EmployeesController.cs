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
    public class EmployeesController : Controller
    {
        private readonly DBHospitalContext _context;

        public EmployeesController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int? id, string? name)
        {

            if (id == null) return RedirectToAction("Index", "Departments");
            ViewBag.DepartmentID = id;
            ViewBag.DepartmentNaming = name;
            var employeesByDepartments = _context.Employees.Where(em =>em.DepartmentId == id).Include(em =>em.Department);
           
            return View(await employeesByDepartments.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create(int DepartmentID)
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewBag.DepartmentId = DepartmentID;
            ViewBag.DepartmentNaming = _context.Departments.Where(d => d.DepartmentId == DepartmentID).FirstOrDefault().DepartmentNaming;
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int DepartmentID, [Bind("EmployeeId,Name,DateOfBirth,DepartmentId")] Employees employees)
        {
            employees.DepartmentId = DepartmentID;
            if (ModelState.IsValid)
            {
                if (_context.Employees.Where(pr => pr.Name == employees.Name).Count() == 0)
                   
                {
                    _context.Add(employees);
                    await _context.SaveChangesAsync();
                    // return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "Employees", new { id = DepartmentID, name = _context.Departments.Where(d => d.DepartmentId == DepartmentID).FirstOrDefault().DepartmentNaming });
                }

            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employees.DepartmentId);
            return View(employees);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employees.DepartmentId);
            return View(employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,DateOfBirth,DepartmentId")] Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Employees.Where(pr => pr.Name == employees.Name).Count() == 0)

                {
                    try
                    {
                        _context.Update(employees);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeesExists(employees.EmployeeId))
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", employees.DepartmentId);
            return View(employees);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
