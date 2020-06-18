using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ActivityViewModel
    {
        public int FK_TeamId { get; set; }

        public List<Activity> Tasks { get; set; }

        public List< Team> Teams { get; set; }
        public Team Team { get; set; }

        public Project Project { get; set; }
    }
}
