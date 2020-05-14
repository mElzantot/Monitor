using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class AdminViewModel
    {
        public List<Department> Departments { get; set; }

        public Department Dept { get; set; }

        [Display(Name ="Team Name")]
        public string teamName { get; set; }
    }
}
