using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ActivityViwewModel
    {
        public int ProjectId { get; set; }
        public List<Act> Acts { set; get; }
        public List<Activity> Activities { set; get; }
    }

    public class Act
    {
        public int id { get; set; }
        public string name { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public int duration { get; set; }

    }
}
