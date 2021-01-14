using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class ChangeEmploeeInProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<Employee> Employes { get; set; }
        public List<int> SelectedEmployee { get; set; } = new List<int>();
    }
}
