using ProjectManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.ViewModels
{
    public class ChangeStatusTaskViewModel
    {
        public int IdTask { get; set; }
        public StatusTask CurrentStatusTask { get; set; }
    }
}
