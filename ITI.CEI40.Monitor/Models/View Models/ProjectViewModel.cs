using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models
{
    public class ProjectViewModel
    {
        public IEnumerable<Project> Projects { get; set; }
        public Project Project { get; set; }
    }
}
