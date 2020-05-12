using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class TeamTasks
    {
        public Activity Task { get; set; }

        public Team Team { get; set; }

        public int TaskID { get; set; }

        public int TeamID { get; set; }


    }
}
