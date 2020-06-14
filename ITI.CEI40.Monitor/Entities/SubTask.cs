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
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        public Activity Task { get; set; }
        [ForeignKey(nameof(Task))]
        public int FK_TaskId { get; set; }

        public ICollection<Claim> Claims { get; set; }

        [DataType(DataType.Date)]
        [Column("Start Date" ,TypeName = "Date")]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Column("End Date", TypeName = "Date")]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }


        public int ActualDuration { get; set; }

        public bool IsUnderWork { get; set; }
        public int Progress { get; set; }
        public Status Status { get; set; }

        
        public Priority Priority { get; set; }


        [Range(0, 100)]
        [Required]
        public int TimeManagement { get; set; }

        [Range(0, 100)]
        [Required]
        public int Complexity { get; set; }

        [Range(0, 100)]
        [Required]
        public int Quality { get; set; }


        //public ICollection<EngineerSubTasks> EngineerSubTasks { get; set; }
        public virtual ICollection<SubTaskSession> SubTaskSession { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public ApplicationUser Engineer { get; set; }
        [ForeignKey("Engineer")]
        public string FK_EngineerID { get; set; }

    }
}
