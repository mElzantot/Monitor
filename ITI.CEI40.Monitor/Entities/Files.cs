using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Files
    {

        [Key]
        public int Id { get; set; }
        //public FileType FileType { get; set; }
        public string FilePath { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentID { get; set; }
        public Comment Comment { get; set; }

        //[ForeignKey("Sender")]
        //public string FK_SenderId { get; set; }

        //public ApplicationUser Sender { get; set; }

        //public string RecieverId { get; set; }

        //public DateTime Time { get; set; }

        //[ForeignKey("Project")]
        //public int? FK_ProjectId { get; set; }
        //public Project Project { get; set; }

        //[ForeignKey("Task")]
        //public int? FK_TaskId { get; set; }
        //public Activity Task { get; set; }


        //[ForeignKey("SubTask")]
        //public int? FK_SubTaskId { get; set; }
        //public SubTask SubTask { get; set; }

    }
}
