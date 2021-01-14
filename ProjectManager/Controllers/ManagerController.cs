using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using ProjectManager.ViewModels;
using System.Threading.Tasks;

namespace ProjectManager.Controllers
{
    [Authorize(Roles = "supervisor")]
    public class ManagerController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;

        public ManagerController(ApplicationContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.Managers.ToListAsync());
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
                Manager manager = new Manager { 
                    Email = createUserVM.Email, 
                    UserName = createUserVM.Email, 
                    Name = createUserVM.Name, 
                    Surname = createUserVM.Surname, 
                    Patronymic = createUserVM.Patronymic 
                };

                var result = await _userManager.CreateAsync(manager, createUserVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(manager, "manager");
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
                Manager manager = await _db.Managers.Include(p=>p.Projects).FirstOrDefaultAsync(p => p.Id == id);
                if (manager != null)
                    return View(manager);
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
                Manager manager = await _db.Managers.FirstOrDefaultAsync(p => p.Id == id);
                if (manager != null)
                    return View(manager);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Manager manager = await _db.Managers.Include(m=>m.Projects).FirstOrDefaultAsync(p => p.Id == id);
                if (manager != null)
                {
                    _db.Managers.Remove(manager);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
