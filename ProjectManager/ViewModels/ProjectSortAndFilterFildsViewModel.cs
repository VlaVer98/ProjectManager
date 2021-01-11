using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class ProjectSortAndFilterFildsViewModel
    {
        public List<Project> Projects { get; set; }
        public DateTime? StartDateWith { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateWith { get; set; }
        public DateTime? EndDateTo { get; set; }
        public int? Priority { get; set; }
        public ProjectSortState NameSort { get; set; }
        public ProjectSortState DataStartSort { get; set; }
        public ProjectSortState DataEndSort { get; set; }
        public ProjectSortState PrioritySort { get; set; }
    }
}
