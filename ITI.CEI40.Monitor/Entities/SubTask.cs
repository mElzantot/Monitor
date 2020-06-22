using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("SubTask")]
    public class SubTask
    {
        public SubTask()
        {
            this.Progress = 0;
            this.SubTaskSession = new HashSet<SubTaskSession>();
            this.Comments = new HashSet<Comment>();
            this.ActualDuration = 0;
            this.Status = Status.Active;
            this.Quality = 0;
            this.TimeManagement = 0;
            this.Complexity = 0;
        }


        [Key]
        [NoColumn]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [NoColumn]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        [NoColumn]
        public string Description { get; set; }

        [NoColumn]
        public Activity Task { get; set; }
        [ForeignKey(nameof(Task))]
        [NoColumn]
        public int FK_TaskId { get; set; }
        [NoColumn]
        public ICollection<Claim> Claims { get; set; }

        [DataType(DataType.Date)]
        [Column("Start Date" ,TypeName = "Date")]
        [Display(Name = "Start Date")]
        [NoColumn]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Column("End Date", TypeName = "Date")]
        [Display(Name = "End Date")]
        [NoColumn]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Date)]
        [Column("Actual End Date", TypeName = "Date")]
        [NoColumn]
        public DateTime? ActualEndDate { get; set; }

        //----------Actual Working Hours
        [LoadColumn(2)]
        public float ActualDuration { get; set; }

        [NoColumn]
        public bool IsUnderWork { get; set; }

        [NoColumn]
        public int Progress { get; set; }

        [NoColumn]
        public Status Status { get; set; }

        [NoColumn]
        public Priority Priority { get; set; }

        [Range(0, 100)]
        [Required]
        [NoColumn]
        public float TimeManagement { get; set; }

        [Range(0, 100)]
        [Required]
        [LoadColumn(0)]
        public float Complexity { get; set; }

        [Range(0, 100)]
        [Required]
        [LoadColumn(1)]
        public float Quality { get; set; }

        //public ICollection<EngineerSubTasks> EngineerSubTasks { get; set; }
        [NoColumn]
        public virtual ICollection<SubTaskSession> SubTaskSession { get; set; }
        //[NoColumn]
        public virtual ICollection<Comment> Comments { get; set; }
        [NoColumn]
        public ApplicationUser Engineer { get; set; }
        [ForeignKey("Engineer")]
        [NoColumn]
        public string FK_EngineerID { get; set; }
    }
}
