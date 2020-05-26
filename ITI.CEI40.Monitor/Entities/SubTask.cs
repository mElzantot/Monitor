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


        //public int EstDuration { get; set; }


        public bool IsUnderWork { get; set; }
        public int Progress { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public int? Evaluation { get; set; }


        //public ICollection<EngineerSubTasks> EngineerSubTasks { get; set; }
        public virtual ICollection<SubTaskSession> SubTaskSession { get; set; }
        public ApplicationUser Engineer { get; set; }
        [ForeignKey("Engineer")]
        public string FK_EngineerID { get; set; }

    }
}
