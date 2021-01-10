using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class ProjectCreationViewModel
    {
        public Project Project { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Employee> Employes { get; set; }
        public int? SelectedManager { get; set; }
        public int[] SelectedEmployes { get; set; }
    }
}
