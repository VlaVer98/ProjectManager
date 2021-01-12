using ProjectManager.Models;
using System.Collections.Generic;

namespace ProjectManager.ViewModels
{
    public class TaskSortAndFilterFildsViewModel
    {
        public List<Task> Tasks { get; set; }
        //фильтры
        public string SurnameEmployee { get; set; }
        public string NameProject { get; set; }
        public StatusTask? StatusTask { get; set; }
        public int? Priority { get; set; }
        //сортировка
        public TaskSortState NameSort { get; set; }
        public TaskSortState SurnameEmployeeSort { get; set; }
        public TaskSortState StatusTaskSort { get; set; }
        public TaskSortState PrioritySort { get; set; }
    }
}
