using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{

    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        {
            this.subTasks = new HashSet<SubTask>();
        }

        public int? FK_TeamID { get; set; }
        public Team Team { get; set; }


        [DataType(DataType.Date)]
        public DataType AvailableTime { get; set; }

        //-------Do we really need this ?!
        public int Workload { get; set; }

        public float TotalEvaluation { get; set; }

        [Required]
        public float SalaryRate { get; set; }

        //---------Many to Many Relation
        //public ICollection<EngineerSubTasks> EngineerSubTasks { get; set; }

        public ICollection<SubTask> subTasks { get; set; }

    }
}
