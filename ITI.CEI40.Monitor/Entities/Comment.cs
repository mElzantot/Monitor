using ITI.CEI40.Monitor.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Sender))]
        public string FK_sender { get; set; }
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Task))]
        public int? FK_TaskID { get; set; }
        public Activity Task { get; set; }


        [ForeignKey(nameof(SubTask))]
        public int? FK_SubTaskId { get; set; }
        public SubTask subTask { get; set; }

        public string comment { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime commentTime { get; set; }

        public CommentLevels commentLevel { get; set; }
        public Files File { get; set; }

    }
}
