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

        IEnumerable<Activity> GetDepartmentTasks(int depid);
        IEnumerable<Activity> GetByProjectId(int id);
        IEnumerable<Activity> GetByProIdAndViewOrder(int proId, int viewOrder);
        List<Activity> GetHoldActiveTasks(int teamId);
        IEnumerable<Activity> GetDepCancelledTasks(int depid);
        Activity GetTaskWithComments(int taskId);
        Activity GetTaskWithProjectAndTeam(int taskId);
        IEnumerable<Activity> Archive(int depid);


    }

}
