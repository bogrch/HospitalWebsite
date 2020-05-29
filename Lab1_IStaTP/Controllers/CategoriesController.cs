using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1_IStaTP;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;

namespace Lab1_IStaTP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DBHospitalContext _context;

        public CategoriesController(DBHospitalContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categories == null)
            {
                return NotFound();
            }

            // return View(categories);
            return RedirectToAction("Index","Diseases",new { id=categories.CategoryId, name=categories.CategoryNaming });
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryNaming")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                if (_context.Categories.Where(pr => pr.CategoryNaming == categories.CategoryNaming).Count() == 0)
                {
                    _context.Add(categories);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
            }
            return View(categories);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryNaming")] Categories categories)
        {
            if (id != categories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Categories.Where(pr => pr.CategoryNaming == categories.CategoryNaming).Count() == 0)
                {
                    try
                    {
                        _context.Update(categories);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoriesExists(categories.CategoryId))
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
                    ModelState.AddModelError("", "Sorry! Doublicate error!");
                }
            }
            return View(categories);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
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
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Categories newcat;
                                var c = (from cat in _context.Categories
                                         where cat.CategoryNaming.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Categories();
                                    newcat.CategoryNaming = worksheet.Name;
                                    if (_context.Categories.Where(pr => pr.CategoryNaming == newcat.CategoryNaming).Count() == 0)
                                    {
                                        //додати в контекст
                                        _context.Categories.Add(newcat);
                                    }
                                }
                                                  
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        
                                        Diseases disease = new Diseases();
                                        disease.DiseaseNaming = row.Cell(1).Value.ToString();
                                        disease.Category = newcat;
                                        if (_context.Diseases.Where(pr => pr.DiseaseNaming == disease.DiseaseNaming).Count() == 0)
                                        {
                                            _context.Diseases.Add(disease);
                                        }
                                       
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
                var categories = _context.Categories.Include("Diseases").ToList();
                
                foreach (var c in categories)
                {
                    var worksheet = workbook.Worksheets.Add(c.CategoryNaming);

                    worksheet.Cell("A1").Value = "Disease Naming";
                    
                   
                    worksheet.Row(1).Style.Font.Bold = true;
                    var diseases = c.Diseases.ToList();

                   
                    for (int i = 0; i < diseases.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = diseases[i].DiseaseNaming;
                        
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Hospital_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
