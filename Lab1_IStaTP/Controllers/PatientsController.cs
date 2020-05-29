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
    public class PatientsController : Controller
    {
        private readonly DBHospitalContext _context;

        public PatientsController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
           var dBHospitalContext = _context.Patients.Include(p => p.PneighbourhoodNavigation);
            return View(await dBHospitalContext.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients
       //         .Include(p => p.PneighbourhoodNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodNaming");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,Psurname,Pname,PdateOfBirth,Pneighbourhood,Paddress")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                if (!((_context.Patients.Where(pr => pr.Psurname == patients.Psurname).Count() != 0) 
                    && (_context.Patients.Where(pr => pr.Pname == patients.Pname).Count() != 0)
                    && (_context.Patients.Where(pr => pr.PdateOfBirth == patients.PdateOfBirth).Count() != 0))) 
                    {
                    _context.Add(patients);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate Error!");
                 //  return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodNaming", patients.Pneighbourhood);
            return View(patients);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients.FindAsync(id);
            if (patients == null)
            {
                return NotFound();
            }
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodNaming", patients.Pneighbourhood);
            return View(patients);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,Psurname,Pname,PdateOfBirth,Pneighbourhood,Paddress")] Patients patients)
        {
            if (id != patients.PatientId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (!((_context.Patients.Where(pr => pr.Psurname == patients.Psurname).Count() != 0)
                    && (_context.Patients.Where(pr => pr.Pname == patients.Pname).Count() != 0)
                    ))
                {
                    try
                    {
                        _context.Update(patients);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PatientsExists(patients.PatientId))
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
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate Error!");
                }
            }
            
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodId", patients.Pneighbourhood);
            return View(patients);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patients = await _context.Patients
               .Include(p => p.PneighbourhoodNavigation)
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patients = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientsExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
