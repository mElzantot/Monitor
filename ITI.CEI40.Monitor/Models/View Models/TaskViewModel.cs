using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class TaskViewModel
    {
        public string taskDescription { get; set; }

        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public DateTime TaskEndDate { get; set; }

        public List<SubTask> SubTasks { get; set; }

    }
}
