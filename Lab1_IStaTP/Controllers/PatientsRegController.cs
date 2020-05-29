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
    public class PatientsRegController : Controller
    {
        private readonly DBHospitalContext _context;

        public PatientsRegController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: PatientsReg
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "PatientRegistrations");
            ViewBag.PatientID = id;
            var patientByReg = _context.Patients.Where(p => p.PatientId == id).Include(p => p.PatientRegistration);
            return View(await patientByReg.ToListAsync());
        }

        // GET: PatientsReg/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: PatientsReg/Create
        public IActionResult Create()
        {
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodId");
            return View();
        }

        // POST: PatientsReg/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,Psurname,Pname,PdateOfBirth,Pneighbourhood,Paddress")] Patients patients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodId", patients.Pneighbourhood);
            return View(patients);
        }

        // GET: PatientsReg/Edit/5
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
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodId", patients.Pneighbourhood);
            return View(patients);
        }

        // POST: PatientsReg/Edit/5
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
            ViewData["Pneighbourhood"] = new SelectList(_context.Neighbourhoods, "NeighbourhoodId", "NeighbourhoodId", patients.Pneighbourhood);
            return View(patients);
        }

        // GET: PatientsReg/Delete/5
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

        // POST: PatientsReg/Delete/5
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
