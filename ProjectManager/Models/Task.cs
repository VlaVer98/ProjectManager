using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public enum StatusTask
    {
        ToDo,
        InProgress,
        Done
    }
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Author { get; set; }
        public Employee Employee { get; set; }
        public StatusTask Status { get; set; }
        public string Comment { get; set; }
        public int Priority { get; set; }
    }
}
