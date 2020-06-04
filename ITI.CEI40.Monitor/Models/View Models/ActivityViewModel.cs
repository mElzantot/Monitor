using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ActivityViewModel
    {
        public int FK_TeamId { get; set; }

        public IEnumerable<Activity> Tasks { get; set; }
        public Activity Task { get; set; }

        public IEnumerable< Team> Teams { get; set; }
        public Team Team { get; set; }

        public Project Project { get; set; }

        public ICollection<IFormFile> files { get; set; }
    }
}
