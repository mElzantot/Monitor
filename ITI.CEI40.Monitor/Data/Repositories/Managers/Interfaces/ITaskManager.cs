using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Entities;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface ITaskManager : IRepository<Activity>
    {
        IEnumerable<Activity> GetAllWithAttributes();
        Activity GetTaskWithProject(int taskId);
        IEnumerable<Activity> GetAllTaskWithTheirProject(int teamId);
        IEnumerable<Activity> GetTasksByTeamID(int teamId);
        IEnumerable<Activity> GetActivitiesFromProject(int projId);


    }

}
