using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("Task")]
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        public Team Team { get; set; }
        [ForeignKey(nameof(Team))]
        public int FK_TeamId { get; set; }


        public ICollection<SubTask> SubTasks { get; set; }


        public Project Project { get; set; }
        [ForeignKey(nameof(Project))]
        public int FK_ProjectId { get; set; }


        [DataType(DataType.Date)]
        [Column("Start Date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [Column("End Date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public float Progress { get; set; }

        public Status Status { get; set; }
        public Priority Priority { get; set; }

        //public ICollection<TeamTasks> TeamTasks { get; set; }


    }
}
