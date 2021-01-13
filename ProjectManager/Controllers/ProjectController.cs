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
    public class ProjectController : Controller
    {
        ApplicationContext _db;

        public ProjectController(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index(
            DateTime? startDateWith, DateTime? startDateTo,
            DateTime? endDateWith, DateTime? endDateTo,
            int? priority,
            ProjectSortState sortOrder = ProjectSortState.NameAsc
            )
        {
            var projects = from project in _db.Projects select project;

            //фильтры
            projects = Project.FilterByStartDate(projects, startDateWith, startDateTo);
            projects = Project.FilterEndDate(projects, endDateWith, endDateTo);
            projects = Project.FilterByPriority(projects, priority);

            //сортировка
            projects = Project.Sort(projects, sortOrder);

            var model = new ProjectSortAndFilterFildsViewModel
            {
                Projects = await projects.ToListAsync(),
                //значение фильтров
                StartDateWith = startDateWith,
                StartDateTo = startDateTo,
                EndDateWith = endDateWith,
                EndDateTo = endDateTo,
                Priority = priority,
                //знчение сортировок
                NameSort = sortOrder == ProjectSortState.NameAsc ? ProjectSortState.NameDesc : ProjectSortState.NameAsc,
                DataStartSort = sortOrder == ProjectSortState.DataStartAsc ? ProjectSortState.DataStartDesc : ProjectSortState.DataStartAsc,
                DataEndSort = sortOrder == ProjectSortState.DataEndAsc ? ProjectSortState.DataEndDesc : ProjectSortState.DataEndAsc,
                PrioritySort = sortOrder == ProjectSortState.PriorityAsc ? ProjectSortState.PriorityDesc : ProjectSortState.PriorityAsc
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var projectCreationVM = new ProjectCreationViewModel
            {
                Managers = await _db.Managers.ToListAsync<Manager>(),
                Employes = await _db.Employes.ToListAsync<Employee>()
            };

            return View(projectCreationVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectCreationViewModel projectCreationVM)
        {
            if (ModelState.IsValid)
            {
                projectCreationVM.Project.ProjectManager = await _db.Managers.FirstOrDefaultAsync(m => m.Id == projectCreationVM.SelectedManager);

                if(projectCreationVM.SelectedEmployes != null)
                {
                    projectCreationVM.Project.ProjectPerformers = new List<Employee>();
                    foreach (var item in projectCreationVM.SelectedEmployes)
                    {
                        Employee employee = await _db.Employes.FirstOrDefaultAsync(e => e.Id == item);
                        if (employee != null)
                            projectCreationVM.Project.ProjectPerformers.Add(employee);
                    }
                }

                _db.Projects.Add(projectCreationVM.Project);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            projectCreationVM.Managers = await _db.Managers.ToListAsync<Manager>();
            projectCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(projectCreationVM);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Project project = await _db.Projects.Include(p=>p.Tasks).Include(p=>p.ProjectManager).Include(p => p.ProjectPerformers).FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                    return View(project);
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var projectCreationVM = new ProjectCreationViewModel
                {
                    Project = await _db.Projects.Include(p => p.ProjectManager).Include(p => p.ProjectPerformers).FirstOrDefaultAsync(p => p.Id == id),
                    Managers = await _db.Managers.ToListAsync<Manager>(),
                    Employes = await _db.Employes.ToListAsync<Employee>()
                };
                if (projectCreationVM.Project != null)
                {
                    projectCreationVM.SelectedManager = projectCreationVM.Project?.ProjectManager?.Id;
                    projectCreationVM.SelectedEmployes = projectCreationVM.Project?.ProjectPerformers?.Select(e => e.Id).ToArray<int>();
                    return View(projectCreationVM);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectCreationViewModel projectCreationVM)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        projectCreationVM.Project.ProjectManager = await _db.Managers.FirstAsync(m => m.Id == projectCreationVM.SelectedManager);

                        _db.Projects.Update(projectCreationVM.Project);
                        await _db.SaveChangesAsync();

                        Project p = await _db.Projects.Include(p => p.ProjectPerformers).FirstOrDefaultAsync(p => p.Id == projectCreationVM.Project.Id);
                        p.ProjectPerformers.Clear();
                        if(projectCreationVM.SelectedEmployes != null && projectCreationVM.SelectedEmployes.Length != 0)
                        {
                            foreach (var item in projectCreationVM.SelectedEmployes)
                            {
                                Employee employee = await _db.Employes.FirstAsync(e => e.Id == item);
                                if (employee != null)
                                    p.ProjectPerformers.Add(employee);
                            }
                        }
                        _db.Projects.Update(p);
                        await _db.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return BadRequest();
                    }
                }

                return RedirectToAction("Index");
            }

            projectCreationVM.Managers = await _db.Managers.ToListAsync<Manager>();
            projectCreationVM.Employes = await _db.Employes.ToListAsync<Employee>();
            return View(projectCreationVM);
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                    return View(project);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == id);
                if (project != null)
                {
                    _db.Projects.Remove(project);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
        
        public async Task<IActionResult> AddTask(int? id)
        {
            if(id != null)
            {
                Project project = await _db.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
                if(project != null)
                {
                    var addTaskToProjectVm = new AddTaskToProjectViewModel
                    {
                        ProjectId = project.Id,
                        ProjectName = project.Name,
                        Tasks = await _db.Tasks.ToListAsync(),
                        SelectedTasks = project.Tasks.Select(t => t.Id).ToList<int>()
                    };

                    return View(addTaskToProjectVm);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AddTaskToProjectViewModel addTaskToProjectVM)
        {
            Project project = await _db.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == addTaskToProjectVM.ProjectId);
            if(project != null)
            {
                try
                {
                    project.Tasks.Clear();

                    foreach (var item in addTaskToProjectVM.SelectedTasks)
                    {
                        project.Tasks.Add(await _db.Tasks.FindAsync(item));
                    }

                    _db.Projects.Update(project);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Details", new { id = project.Id});
                }
                catch (Exception)
                {
                    return BadRequest();
                } 
            }
            return BadRequest();
        }
    }
}
