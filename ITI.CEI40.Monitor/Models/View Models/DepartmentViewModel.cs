using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class DepartmentViewModel
    {
        public List<Department> Departments { get; set; }

        public Department Dept { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Team Leader")]
        public int TeamLeaderId { get; set; }

    }
}
