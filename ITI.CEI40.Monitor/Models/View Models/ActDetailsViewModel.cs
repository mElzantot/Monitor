using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ActDetailsViewModel
    {
        public Activity Task { get; set; }
        public List<Comment> HighComments { get; set; }
        public List<Comment> MedComments { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public int FK_TeamId { get; set; }
        public Team Team { get; set; }

    }
}
