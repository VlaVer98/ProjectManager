using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using System.Threading.Tasks;

namespace ProjectManager.Controllers
{
    public class ManagerController : Controller
    {
        private ApplicationContext _db;
        public ManagerController(ApplicationContext context)
        {
            _db = context;
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
        public async Task<IActionResult> Create(Manager manager)
        {
            if (ModelState.IsValid)
            {
                _db.Managers.Add(manager);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(manager);
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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(Manager manager)
        {
            if (ModelState.IsValid)
            {
                _db.Managers.Update(manager);
                await _db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = manager.Id });
            }

            return View(manager);
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
                Manager manager = await _db.Managers.FirstOrDefaultAsync(p => p.Id == id);
                if (manager != null)
                {
                    _db.Users.Remove(manager);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
