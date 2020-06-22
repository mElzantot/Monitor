using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class SubTaskSubmitVM
    {
        public List<ApplicationUser> teamMembers { get; set; }

        [Required]
        public float Complexity { get; set; }
        [Required]
        public float Quality { get; set; }
        [Required]
        [Display(Name = "Time Mangment")]
        public float TimeMangment { get; set; }
        [Required]
        public int subTaskId { get; set; }

    }
}
