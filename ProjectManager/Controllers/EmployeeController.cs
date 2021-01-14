using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Authorize(Roles = "supervisor")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;

        public EmployeeController(ApplicationContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Create(CreateUserViewModel createUserVM)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    Email = createUserVM.Email,
                    UserName = createUserVM.Email,
                    Name = createUserVM.Name,
                    Surname = createUserVM.Surname,
                    Patronymic = createUserVM.Patronymic
                };

                var result = await _userManager.CreateAsync(employee, createUserVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employee, "manager");
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(createUserVM);
            }

            return View(createUserVM);
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

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel editUserVM)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(editUserVM.Id);
                if (user != null)
                {
                    user.Name = editUserVM.Name;
                    user.Surname = editUserVM.Surname;
                    user.Patronymic = editUserVM.Patronymic;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(editUserVM);
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
