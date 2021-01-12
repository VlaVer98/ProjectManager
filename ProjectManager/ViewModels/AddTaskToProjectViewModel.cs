using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.ViewModels
{
    public class AddTaskToProjectViewModel
    {
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<Task> Tasks { get; set; }
        public List<int> SelectedTasks { get; set; } = new List<int>();
    }
}
