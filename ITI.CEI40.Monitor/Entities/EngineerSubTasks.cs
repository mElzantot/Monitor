using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class EngineerSubTasks
    {
        public virtual ApplicationUser Engineer { get; set; }
        public virtual SubTask SubTask { get; set; }
        public string EngineerID { get; set; }
        public int SubTaskID { get; set; }
        public virtual Status Status { get; set; }
        public float Evaluation { get; set; }


    }
}
