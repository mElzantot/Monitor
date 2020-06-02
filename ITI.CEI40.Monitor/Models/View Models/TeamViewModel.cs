using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class TeamViewModel
    {
        public List<Team> teams { get; set; }

        public string DepName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int TeamLeaderId { get; set; }

        [Required]
        [Display(Name ="Department")]
        public int FK_DepartmentId { get; set; }


    }
}
