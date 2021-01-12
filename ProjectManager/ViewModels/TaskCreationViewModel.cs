using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.ViewModels
{
    public class TaskCreationViewModel
    {
        public Task Task { get; set; }
        public List<Employee> Employes { get; set; }
        public int? SelectedEmployee { get; set; }

    }
}
