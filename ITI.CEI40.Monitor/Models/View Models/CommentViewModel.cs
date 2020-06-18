using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class CommentViewModel
    {
        public List<Comment> Comments { get; set; }
        public List<SubTask> SubTasks { get; set; }
        public string Sender { get; set; }
    }
}
