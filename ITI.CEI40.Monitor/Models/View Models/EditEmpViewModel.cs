using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class EditEmpViewModel
    {
        #region Employee Date
        public string EmpId { get; set; }

        public string EmpName { get; set; }

        [Required]
        [Range(2000, 60000)]
        public float Salary { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Mail")]
        [Required]
        public string EMail { get; set; }
        #endregion

        [Required]
        public int DepId { get; set; }

        [Display(Name = "Department Name")]
        public string DepName { get; set; }

        [Required]
        public int TeamId { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        public List<Department> Departments { get; set; }
        public List<Team> Teams { get; set; }
    }
}
