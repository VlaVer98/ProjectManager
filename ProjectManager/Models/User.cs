using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Patronymic { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
