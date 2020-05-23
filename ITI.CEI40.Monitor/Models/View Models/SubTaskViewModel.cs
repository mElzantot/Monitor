using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class SubTaskViewModel
    {


        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int FK_TaskId { get; set; }

        [Required]
        [Display(Name ="Start Date") ]
        public string StartDate { get; set; }

        [Display(Name ="Due Date") ]
        public string EndDate { get; set; }
        public int Progress { get; set; }
        public Status Status { get; set; }
        [Required]
        public Priority Priority { get; set; }

        public List<ApplicationUser> TeamMembers { get; set; }

        [Display(Name = "Assigned to")]
        public string AssigneeId { get; set; }


    }
}
