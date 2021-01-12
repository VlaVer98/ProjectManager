using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public enum TaskSortState
    {
        NameAsc,
        NameDesc,
        SurnameEmployeeAsc,
        SurnameEmployeeDesc,
        StatusAsc,
        StatusDesc,
        PriorityAsc,
        PriorityDesc,
    }

    public enum StatusTask
    {
        ToDo,
        InProgress,
        Done
    }
    public class Task
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public User Author { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        [Required]
        public StatusTask Status { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }

        public static IQueryable<Task> FilterByName(IQueryable<Task> query, string name)
        {
            if(name != null)
            {
                query = query.Where(t => t.Name.Contains(name));
            }
            return query;
        }

        public static IQueryable<Task> FilterBySurnameEmployee(IQueryable<Task> query, string surname)
        {
            if(surname != null)
            {
                query = query.Where(t => t.Employee.Surname.Contains(surname));
            }

            return query;
        }

        public static IQueryable<Task> FilterByStatusTask(IQueryable<Task> query, StatusTask? statusTask)
        {
            if(statusTask != null)
            {
                query = query.Where(t => t.Status == statusTask);
            }
            return query;
        }

        public static IQueryable<Task> FilterByPriority(IQueryable<Task> query, int? priority)
        {
            if(priority != null)
            {
                query = query.Where(t => t.Priority == priority);
            }
            return query;
        }

        public static IQueryable<Task> Sort(IQueryable<Task> query, TaskSortState? sortTask)
        {
            if(sortTask != null)
            {
                query = sortTask switch
                {
                    TaskSortState.NameDesc => query.OrderByDescending(t => t.Name),
                    TaskSortState.SurnameEmployeeAsc => query.OrderBy(t => t.Employee.Surname),
                    TaskSortState.SurnameEmployeeDesc => query.OrderByDescending(t => t.Employee.Surname),
                    TaskSortState.StatusAsc => query.OrderBy(t => t.Status),
                    TaskSortState.StatusDesc => query.OrderByDescending(t => t.Status),
                    TaskSortState.PriorityAsc => query.OrderBy(t => t.Priority),
                    TaskSortState.PriorityDesc => query.OrderByDescending(t => t.Priority),

                    _ => query.OrderBy(t => t.Name),
                };
            }
            return query;
        }
    }
}
