using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CompanyCustomer { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CompanyPerformer { get; set; }
        public Manager ProjectManager { get; set; }
        public List<Employee> ProjectPerformers { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }
        public List<Task> Tasks { get; set; }

        public static IQueryable<Project> FilterByStartDate(IQueryable<Project> query, DateTime? startDateWith, DateTime? startDateTo)
        {
            if (startDateWith != null && startDateTo != null)
                query = from project in query where project.StartDate >= startDateWith && project.StartDate <= startDateTo select project;
            else if (startDateWith != null)
                query = from project in query where project.StartDate >= startDateWith select project;
            else if (startDateTo != null)
                query = from project in query where project.StartDate <= startDateTo select project;

            return query;
        }

        public static IQueryable<Project> FilterEndDate(IQueryable<Project> query, DateTime? endDateWith, DateTime? endDateTo)
        {
            if (endDateWith != null && endDateTo != null)
                query = from project in query where project.EndDate >= endDateWith && project.EndDate <= endDateTo select project;
            else if (endDateWith != null)
                query = from project in query where project.EndDate >= endDateWith select project;
            else if (endDateTo != null)
                query = from project in query where project.EndDate <= endDateTo select project;

            return query;
        }

        public static IQueryable<Project> FilterByPriority(IQueryable<Project> query, int? priority)
        {
            if (priority != null)
                query = from project in query where project.Priority == priority select project;

            return query;
        }
    }
}
