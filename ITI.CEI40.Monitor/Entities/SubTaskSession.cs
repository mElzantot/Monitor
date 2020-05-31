using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class SubTaskSession
    {
        public SubTaskSession()
        {
            this.SessDuration = 0;
        }

        [Key]
        public int ID { get; set; }
        public SubTask SubTask { get; set; }
        [ForeignKey("SubTask")]
        public int FK_SubTaskID { get; set; }

        public ApplicationUser Employee { get; set; }
        [ForeignKey("Employee")]
        [Required]
        public string EmpId { get; set; }


        [DataType(DataType.DateTime)]
        [Column("Start Date")]
        [Display(Name = "Start Date")]
        public DateTime SessStartDate { get; set; }


        [DataType(DataType.DateTime)]
        [Column("End Date")]
        [Display(Name = "End Date")]
        public DateTime? SessEndtDate { get; set; }

        public int SessDuration { get; set; }

    }
}
