using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ProjectManager.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Patronymic { get; set; }

        public string GetFullName()
        {
            StringBuilder fullName = new StringBuilder();
            fullName.AppendFormat("{0} {1} {2}", Surname, Name, Patronymic);

            return fullName.ToString();
        }
    }
}
