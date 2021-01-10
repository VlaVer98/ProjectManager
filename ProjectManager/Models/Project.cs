using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CompanyCustomer { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CompanyPerformer { get; set; }
        public Manager ProjectManager { get; set; }
        public List<Employee> ProjectPerformers { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Required]
        [Range(1, 3)]
        public int Priority { get; set; }
    }
}
