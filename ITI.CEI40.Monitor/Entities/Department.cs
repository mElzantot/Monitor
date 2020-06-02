using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Department
    {

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Display(Name = "Department Manager")]
        public string FK_ManagerID { get; set; }
        public ApplicationUser Manager { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<DepartmentProjects> DepartmentProjects { get; set; }

        public ICollection<Activity> Activities { get; set; }

    }
}
