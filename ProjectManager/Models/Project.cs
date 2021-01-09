using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyCustomer { get; set; }
        public string CompanyPerformer { get; set; }
        public Manager ProjectManager { get; set; }
        public List<Employee> ProjectPerformers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
    }
}
