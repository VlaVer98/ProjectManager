using ProjectManager.Models;
using System.Collections.Generic;

namespace ProjectManager.ViewModels
{
    public class TaskSortAndFilterFildsViewModel
    {
        public List<Task> Tasks { get; set; }
        public string SurnameEmployee { get; set; }
        public string NameProject { get; set; }
        public StatusTask? StatusTask { get; set; }
        public int? Priority { get; set; }
    }
}
