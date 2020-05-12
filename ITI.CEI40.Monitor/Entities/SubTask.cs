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

        [DataType(DataType.DateTime)]
        [Column("Start Date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.DateTime)]
        [Column("Estimated Duration")]
        [Display(Name = "Estimated Duration")]
        public DateTime EstDuration { get; set; }
        public float Progress { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }


        public ICollection<EngineerSubTasks> EngineerSubTasks { get; set; }

    }
}
