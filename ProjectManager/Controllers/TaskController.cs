using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using ProjectManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Controllers
{
    public class TaskController : Controller
    {
        ApplicationContext _db;
        
        public TaskController(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Tasks.Include(t=>t.Employee).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var taskCreationVM = new TaskCreationViewModel
            {
                Employes = await _db.Employes.ToListAsync<Employee>()
            };

            return View(taskCreationVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreationViewModel taskCreationVM)
        {
            if (ModelState.IsValid)
            {
                _db.Tasks.Add(taskCreationVM.Task);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            taskCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(taskCreationVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id != null)
            {
                var task = await _db.Tasks.Include(t=>t.Author).Include(t=>t.Employee).FirstOrDefaultAsync(t=>t.Id==id);
                if(task != null)
                    return View(task);
            }

            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id != null)
            {
                var task = await _db.Tasks.Include(t => t.Author).Include(t => t.Employee).FirstOrDefaultAsync(t => t.Id == id);

                if(task != null)
                {
                    var taskCreatinVM = new TaskCreationViewModel
                    {
                        Task = task,
                        Employes = await _db.Employes.ToListAsync()
                    };

                    return View(taskCreatinVM);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskCreationViewModel taskCreationVM)
        {
            if (ModelState.IsValid)
            {
                _db.Update(taskCreationVM.Task);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            taskCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(taskCreationVM);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                if (task != null)
                    return View(task);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var task = await _db.Tasks.FirstOrDefaultAsync(p => p.Id == id);
                if (task != null)
                {
                    _db.Tasks.Remove(task);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
