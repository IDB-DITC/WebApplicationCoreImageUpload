using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MyDb _context;
        private readonly IWebHostEnvironment _enc;

        public EmployeesController(MyDb context, IWebHostEnvironment enc)
        {
            _context = context;
            _enc = enc;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {

            //var data = await _context.Employees.ToListAsync();

            var data = await _context.Employees.FromSql<Employee>($"exec SpGetEmployee ").ToListAsync();

            return View(data);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);


            var employee = _context.Employees.FromSql<Employee>($"exec SpGetEmployee {id}").AsEnumerable().FirstOrDefault();






            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,ImagePath, ImageFile")] Employee employee)
        {


            if (employee.ImageFile != null)
            {



                employee.ImagePath = "\\Image\\" + employee.ImageFile.FileName;


                string serverPath = _enc.WebRootPath + employee.ImagePath;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await employee.ImageFile.CopyToAsync(stream);


            }





            if (ModelState.IsValid)
            {
                //_context.Add(employee);
                ////_context.
                //await _context.SaveChangesAsync();


                var rows = await _context.Database.ExecuteSqlRawAsync("exec SpCreateEmployee @p0, @p1",  employee.EmployeeName, employee.ImagePath);
                if (rows > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var employee = await _context.Employees.FindAsync(id);

            var employee = _context.Employees.FromSql<Employee>($"exec SpGetEmployee {id}").AsEnumerable().FirstOrDefault();

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,ImagePath, ImageFile")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }


            if (employee.ImageFile != null)
            {



                employee.ImagePath = "\\Image\\" + employee.ImageFile.FileName;


                string serverPath = _enc.WebRootPath + employee.ImagePath;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await employee.ImageFile.CopyToAsync(stream);


            }




            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(employee);
                    //await _context.SaveChangesAsync();

                    var rows = await _context.Database.ExecuteSqlRawAsync("exec SpUpdateEmployee @p0, @p1, @p2", employee.EmployeeId,employee.EmployeeName, employee.ImagePath);
                    if (rows > 0)
                        return RedirectToAction(nameof(Index));



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var employee = await _context.Employees.FindAsync(id);
            //if (employee != null)
            //{
            //    _context.Employees.Remove(employee);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));


            var rows = await _context.Database.ExecuteSqlRawAsync($"exec SpDeleteEmployee @p0", id);
            //if (rows > 0)
            return RedirectToAction(nameof(Index));


        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
