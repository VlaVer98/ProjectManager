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
    [Authorize]
    public class TaskController : Controller
    {
        readonly ApplicationContext _db;
        readonly UserManager<User> _userManager;

        public TaskController(ApplicationContext context, UserManager<User> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "supervisor, manager, employee")]
        public async Task<IActionResult> Index(
            string surnameEmployee, string nameProject, 
            StatusTask? statusTask, int? priority,
            TaskSortState taskOrder = TaskSortState.NameAsc
            )
        {
            var tasks = _db.Tasks.Include(t => t.Employee).Include(t=>t.Author).AsQueryable();

            //фильтры
            tasks = Models.Task.FilterByName(tasks, nameProject?.Trim());
            tasks = Models.Task.FilterBySurnameEmployee(tasks, surnameEmployee?.Trim());
            tasks = Models.Task.FilterByStatusTask(tasks, statusTask);
            tasks = Models.Task.FilterByPriority(tasks, priority);

            //сортировка
            tasks = Models.Task.Sort(tasks, taskOrder);

            var taskSortAndFilterFildsVM = new TaskSortAndFilterFildsViewModel
            {
                Tasks = await tasks.ToListAsync(),
                //значение фильтров
                NameProject = nameProject?.Trim(),
                SurnameEmployee = surnameEmployee?.Trim(),
                StatusTask = statusTask,
                Priority = priority,
                //значение сортировок
                NameSort = taskOrder == TaskSortState.NameAsc ? TaskSortState.NameDesc : TaskSortState.NameAsc,
                SurnameEmployeeSort = taskOrder == TaskSortState.SurnameEmployeeAsc ? TaskSortState.SurnameEmployeeDesc : TaskSortState.SurnameEmployeeAsc,
                StatusTaskSort = taskOrder == TaskSortState.StatusAsc ? TaskSortState.StatusDesc : TaskSortState.StatusAsc,
                PrioritySort = taskOrder == TaskSortState.PriorityAsc ? TaskSortState.PriorityDesc : TaskSortState.PriorityAsc
            };

            ViewData["userId"] = _userManager.GetUserId(User);

            return View(taskSortAndFilterFildsVM);
        }

        [Authorize(Roles = "supervisor, manager")]
        public async Task<IActionResult> Create()
        {
            var taskCreationVM = new TaskCreationViewModel
            {
                Employes = await _db.Employes.ToListAsync<Employee>()
            };

            return View(taskCreationVM);
        }

        [Authorize(Roles = "supervisor, manager")]
        [HttpPost]
        public async Task<IActionResult> Create(TaskCreationViewModel taskCreationVM)
        {
            if (ModelState.IsValid)
            {
                User authUser = await _userManager.GetUserAsync(User);
                taskCreationVM.Task.Author = authUser;

                _db.Tasks.Add(taskCreationVM.Task);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            taskCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(taskCreationVM);
        }

        [Authorize(Roles = "supervisor, manager, employee")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["userId"] = _userManager.GetUserId(User);

            if (id != null)
            {
                var task = await _db.Tasks.Include(t=>t.Author).Include(t=>t.Employee).FirstOrDefaultAsync(t=>t.Id==id);
                if(task != null)
                    return View(task);
            }

            return NotFound();
        }

        [Authorize(Roles = "supervisor, manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id != null)
            {
                Models.Task task = null;

                if (User.IsInRole("supervisor"))
                    task = await _db.Tasks.Include(t => t.Author).Include(t => t.Employee).
                        FirstOrDefaultAsync(t => t.Id == id);
                else
                    task = await _db.Tasks.Include(t => t.Author).Include(t => t.Employee).
                        FirstOrDefaultAsync(t => t.Id == id && t.Author.Id.ToString() == _userManager.GetUserId(User));

                if(task != null)
                {
                    var taskCreatinVM = new TaskCreationViewModel
                    {
                        Task = task,
                        Employes = await _db.Employes.ToListAsync()
                    };

                    return View(taskCreatinVM);
                }

                return Unauthorized();
            }
            return NotFound();
        }

        [Authorize(Roles = "supervisor, manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(TaskCreationViewModel taskCreationVM)
        {
            if (ModelState.IsValid)
            {
                Models.Task taskIsUser = await _db.Tasks.Include(t => t.Author).
                    Where(t=>t.Id == taskCreationVM.Task.Id)
                    .AsNoTracking().FirstOrDefaultAsync();

                if (taskIsUser == null)
                    return BadRequest();
                else if (!User.IsInRole("supervisor") && taskIsUser.Author.Id.ToString() != _userManager.GetUserId(User))
                    return Unauthorized();

                _db.Update(taskCreationVM.Task);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            taskCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(taskCreationVM);
        }

        [Authorize(Roles = "supervisor, manager")]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Models.Task task = null;

                if (User.IsInRole("supervisor"))
                    task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
                else
                    task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.Author.Id.ToString() == _userManager.GetUserId(User));

                if (task != null)
                    return View(task);
            }
            return NotFound();
        }

        [Authorize(Roles = "supervisor, manager")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Models.Task task = null;

                if (User.IsInRole("supervisor"))
                    task = await _db.Tasks.Include(t=>t.Author).FirstOrDefaultAsync(t => t.Id == id);
                else
                    task = await _db.Tasks.Include(t => t.Author).FirstOrDefaultAsync(t => t.Id == id && t.Author.Id.ToString() == _userManager.GetUserId(User));

                if (task == null)
                    return BadRequest();
                else if (!User.IsInRole("supervisor") && task.Author.Id.ToString() != _userManager.GetUserId(User))
                    return Unauthorized();

                _db.Tasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [Authorize(Roles = "supervisor, manager, employee")]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if(task != null)
            {
                var changeStatusTaskVM = new ChangeStatusTaskViewModel
                {
                    IdTask = task.Id,
                    CurrentStatusTask = task.Status
                };

                return View(changeStatusTaskVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "supervisor, manager, employee")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeStatusTaskViewModel changeStatusTaskVM)
        {
            if (ModelState.IsValid)
            {
                Models.Task taskIsUser = await _db.Tasks.Include(t => t.Author).Include(t=>t.Employee)
                    .Where(t => t.Id == changeStatusTaskVM.IdTask)
                    .FirstOrDefaultAsync();

                if (taskIsUser == null)
                    return BadRequest();

                if(User.IsInRole("supervisor") ||
                    User.IsInRole("manager") ||
                    User.IsInRole("employee") && taskIsUser.Employee.Id.ToString() == _userManager.GetUserId(User))
                {
                    taskIsUser.Status = changeStatusTaskVM.CurrentStatusTask;

                    _db.Update(taskIsUser);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                return Unauthorized();
            }

            return View(changeStatusTaskVM);
        }
    }
}
