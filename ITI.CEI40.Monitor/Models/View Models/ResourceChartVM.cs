using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ResourceChartVM
    {
        public List<ApplicationUser> Employees { get; set; }

        [Required]
        [Display(Name = "Engineer")]
        public string EmployeeID { get; set; }

        [Required]
        public float Complexity { get; set; }

        public float Duration { get; set; }

        public float Quality { get; set; }

    }
}
