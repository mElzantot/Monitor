using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class EngineerSubTasksManager:Reposiotry<ApplicationDbContext, EngineerSubTasks>,IEngineerSubTasksManager
    {
        public EngineerSubTasksManager(ApplicationDbContext context):base(context)
        {

        }


        public IEnumerable<EngineerSubTasks> GetEngineerswithsubtask(int SubTaskId)
        {
            return set.Where(st => st.SubTaskID == SubTaskId).ToList();
        }


        public IEnumerable<EngineerSubTasks> GetSubTasksFromEngineerId(string EngId)
        {
            return set.Where(e => e.EngineerID == EngId).Include(e => e.SubTask).ThenInclude(s => s.Task).ThenInclude(p => p.Project);
        }




        public void RemoveEngineerSubTask(int stId)
        {
            set.RemoveRange(set.Where(st => st.SubTaskID == stId));
        }
    }
}
