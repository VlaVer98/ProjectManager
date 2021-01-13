using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Controllers
{
    public class EmployeeController : Controller
    {

            private ApplicationContext _db;
            public EmployeeController(ApplicationContext context)
            {
                _db = context;
            }

            public async Task<IActionResult> Index()
            {
                return View(await _db.Employes.ToListAsync());
            }
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _db.Employes.Add(employee);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(employee);
            }
            public async Task<IActionResult> Details(int? id)
            {
                if (id != null)
                {
                    Employee employee = await _db.Employes.Include(p => p.Projects).Include(p=>p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
                    if (employee != null)
                        return View(employee);
                }
                return NotFound();
            }
            public async Task<IActionResult> Edit(int? id)
            {
                if (id != null)
                {
                    Employee employee = await _db.Employes.FirstOrDefaultAsync(p => p.Id == id);
                    if (employee != null)
                        return View(employee);
                }
                return NotFound();
            }
            [HttpPost]
            public async Task<IActionResult> Edit(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _db.Employes.Update(employee);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = employee.Id });
                }

                return View(employee);
            }

            [HttpGet]
            [ActionName("Delete")]
            public async Task<IActionResult> ConfirmDelete(int? id)
            {
                if (id != null)
                {
                    Employee employee = await _db.Employes.FirstOrDefaultAsync(p => p.Id == id);
                    if (employee != null)
                        return View(employee);
                }
                return NotFound();
            }

            [HttpPost]
            public async Task<IActionResult> Delete(int? id)
            {
                if (id != null)
                {
                    Employee employee = await _db.Employes.Include(e=>e.Tasks).FirstOrDefaultAsync(p => p.Id == id);
                    if (employee != null)
                    {
                        _db.Employes.Remove(employee);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                return NotFound();
            }
        }
}
