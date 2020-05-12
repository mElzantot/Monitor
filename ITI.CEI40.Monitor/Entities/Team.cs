using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Team
    {
        public Team()
        {
            this.Engineers = new HashSet<ApplicationUser>();
            //this.Tasks = new HashSet<Activity>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ApplicationUser TeamLeader { get; set; }
        public string FK_TeamLeaderId { get; set; }


        public Department Department { get; set; }
        [ForeignKey(nameof(Department))]
        public int FK_DepartmentId { get; set; }

        public ICollection<ApplicationUser> Engineers { get; set; }

        public ICollection<Activity> Tasks { get; set; }

        //public ICollection<TeamTasks> TeamTasks { get; set; }

    }
}
