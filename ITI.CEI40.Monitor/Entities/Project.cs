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

        [ForeignKey("ProjectManager")]
        public string FK_Manager { get; set; }
        public ApplicationUser Manager { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [MaxLength(30)]
        public string Owner { get; set; }

        public float? TotalBudget { get; set; } //---1000

        //----Estimated Budget
        //public float? EstimatedBudget { get; set;}

        public float? Income { get; set; }      //--Invoices
        public float? Outcome { get; set; }     //---Actual

        public Status? Status { get; set; }
        public Priority? Priority { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public int? EstimatedDuration { get; set; }

        [NotMapped]
        public int ActualDuration { get; set; }

        public ICollection<DepartmentProjects> DepartmentProjects { get; set; }

        public ICollection<Activity> Tasks { get; set; }

    }
}
