using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class TeamTasksManager:Reposiotry<ApplicationDbContext,TeamTasks>,ITeamTasksManager
    {
        public TeamTasksManager(ApplicationDbContext context):base(context)
        {

        }

        public void RemoveTeamTasks(int taskId)
        {
            set.RemoveRange(set.Where(t => t.TaskID == taskId));
        }
    }
}
