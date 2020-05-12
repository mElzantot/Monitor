using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("Project")]
    public class Project
    {

        public Project()
        {
            this.Tasks = new HashSet<Activity>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }


        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string Owner { get; set; }

        [Required]
        public float TotalBudget { get; set; }
        public float Income { get; set; }
        public float Outcome { get; set; }

        public float Progress { get; set; }

        public Status Status { get; set; }
        public Priority Priority { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public float EstimatedDuration { get; set; }

        [NotMapped]
        public float ActualDuration { get; set; }

        public ICollection<DepartmentProjects> DepartmentProjects { get; set; }

        public ICollection<Activity> Tasks { get; set; }

    }
}
