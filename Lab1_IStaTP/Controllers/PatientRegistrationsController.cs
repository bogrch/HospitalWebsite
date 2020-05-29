using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_IStaTP;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace Lab1_IStaTP.Controllers
{
    [Authorize(Roles="admin")]
    public class PatientRegistrationsController : Controller
    {
        private readonly DBHospitalContext _context;

        public PatientRegistrationsController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: PatientRegistrations
        public async Task<IActionResult> Index()
        {
            var dBHospitalContext = _context.PatientRegistration.Include(p => p.Complexity).Include(p => p.Department).Include(p => p.Disease).Include(p => p.Patient);
            return View(await dBHospitalContext.ToListAsync());
        }

        // GET: PatientRegistrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRegistration = await _context.PatientRegistration
                .Include(p => p.Complexity)
                .Include(p => p.Department)
                .Include(p => p.Disease)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (patientRegistration == null)
            {
                return NotFound();
            }

            //return View(patientRegistration);
            return RedirectToAction("Index", "PatientsReg", new { id = patientRegistration.PatientId });
        }

        // GET: PatientRegistrations/Create
        public IActionResult Create()
        {
            
            ViewData["ComplexityId"] = new SelectList(_context.Compexities, "ComplexityId", "ComplexityNaming");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentNaming");
            ViewData["DiseaseId"] = new SelectList(_context.Diseases, "DiseaseId", "DiseaseNaming");
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId","DisplayName2");
           
            return View();
        }

        // POST: PatientRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,PatientId,DiseaseId,DepartmentId,ComplexityId,PrdateOfStart,PrdateOfDischarge")] PatientRegistration patientRegistration)
        {
            if (ModelState.IsValid)
            {
                if (!((_context.PatientRegistration.Where(pr => pr.PatientId == patientRegistration.PatientId).Count() != 0)
                     && (_context.PatientRegistration.Where(pr => pr.DiseaseId == patientRegistration.DiseaseId).Count() != 0)    
                     && (_context.PatientRegistration.Where(pr => pr.PrdateOfStart == patientRegistration.PrdateOfStart).Count() != 0)
                     && (_context.PatientRegistration.Where(pr => pr.DepartmentId == patientRegistration.DepartmentId).Count() != 0)
                    
                    ))
                {
                   if(patientRegistration.PrdateOfDischarge!= null )
                    {
                        if (patientRegistration.PrdateOfStart < patientRegistration.PrdateOfDischarge)
                        {
                            _context.Add(patientRegistration);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("", "Sorry! The discharge date cannot be earlier than starting date!");
                        }
                    }
                   else
                    {
                        _context.Add(patientRegistration);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate Error!");
                }
            }
            ViewData["ComplexityId"] = new SelectList(_context.Compexities, "ComplexityId", "ComplexityNaming", patientRegistration.ComplexityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentNaming", patientRegistration.DepartmentId);
            ViewData["DiseaseId"] = new SelectList(_context.Diseases, "DiseaseId", "DiseaseNaming", patientRegistration.DiseaseId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "DisplayName2", patientRegistration.PatientId);
          
            return View(patientRegistration);
        }

        // GET: PatientRegistrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRegistration = await _context.PatientRegistration.FindAsync(id);
            if (patientRegistration == null)
            {
                return NotFound();
            }
            ViewData["ComplexityId"] = new SelectList(_context.Compexities, "ComplexityId", "ComplexityNaming", patientRegistration.ComplexityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentNaming", patientRegistration.DepartmentId);
            ViewData["DiseaseId"] = new SelectList(_context.Diseases, "DiseaseId", "DiseaseNaming", patientRegistration.DiseaseId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "DisplayName2", patientRegistration.PatientId);
            return View(patientRegistration);
        }

        // POST: PatientRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,PatientId,DiseaseId,DepartmentId,ComplexityId,PrdateOfStart,PrdateOfDischarge")] PatientRegistration patientRegistration)
        {
            if (id != patientRegistration.AccountId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (!(_context.PatientRegistration.Where(pr => pr.PatientId == patientRegistration.PatientId).Count() != 0)
                     && (_context.PatientRegistration.Where(pr => pr.DiseaseId == patientRegistration.DiseaseId).Count() != 0)
                     && (_context.PatientRegistration.Where(pr => pr.PrdateOfStart == patientRegistration.PrdateOfStart).Count() != 0)
                     && (_context.PatientRegistration.Where(pr => pr.DepartmentId == patientRegistration.DepartmentId).Count() != 0)

                    )
                {
                    if (patientRegistration.PrdateOfDischarge != null)
                    {
                        if (patientRegistration.PrdateOfStart < patientRegistration.PrdateOfDischarge)
                        {
                            try
                            {
                                _context.Update(patientRegistration);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                if (!PatientRegistrationExists(patientRegistration.AccountId))
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
                            ModelState.AddModelError("", "Sorry! The discharge date cannot be earlier than starting date!");
                        }
                    }
                    else
                    {
                        try
                        {
                            _context.Update(patientRegistration);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!PatientRegistrationExists(patientRegistration.AccountId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
            }

            
            
            ViewData["ComplexityId"] = new SelectList(_context.Compexities, "ComplexityId", "ComplexityNaming", patientRegistration.ComplexityId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentNaming", patientRegistration.DepartmentId);
            ViewData["DiseaseId"] = new SelectList(_context.Diseases, "DiseaseId", "DiseaseNaming", patientRegistration.DiseaseId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientRegistration.PatientId);
            return View(patientRegistration);
        }

        // GET: PatientRegistrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patientRegistration = await _context.PatientRegistration
                .Include(p => p.Complexity)
                .Include(p => p.Department)
                .Include(p => p.Disease)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (patientRegistration == null)
            {
                return NotFound();
            }

            return View(patientRegistration);
        }

        // POST: PatientRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patientRegistration = await _context.PatientRegistration.FindAsync(id);
            _context.PatientRegistration.Remove(patientRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientRegistrationExists(int id)
        {
            return _context.PatientRegistration.Any(e => e.AccountId == id);
        }
    }
}
