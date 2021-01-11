using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Employee : User
    {
        public List<Project> Projects { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
