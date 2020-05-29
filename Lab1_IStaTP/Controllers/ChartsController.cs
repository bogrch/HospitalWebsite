using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1_IStaTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBHospitalContext _context;

        public ChartsController(DBHospitalContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var departments = _context.Departments.Include(d => d.Employees).ToList();
            List<object> DepList = new List<object>();
            DepList.Add(new[] { "Department", "Quantity of employees" });

            foreach (var d in departments)
            {
                DepList.Add(new object[] { d.DepartmentNaming, d.Employees.Count() });

            }
            return new JsonResult(DepList);
        }
    }
}