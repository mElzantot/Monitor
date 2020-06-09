using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface ISubTaskManager : IRepository<SubTask>
    {
        SubTask GetClaims(int id);
        IEnumerable<SubTask> GetSubTasksFromTask(int taskId);

        IEnumerable<SubTask> GetSubTasksByEngineerId(string engineerId);

        List<SubTask> GetSubTasksByTaskId(int taskId);
        SubTask GetSubTaskIncludingTask(int subTaskId);
        SubTask GetSubTaskIncludingProject(int subTaskId);

        List<SubTask> GetEngineerSubTasks(string EngineerId);
        SubTask GetSubTaskWithEngineer(int id);
        IEnumerable<SubTask> GetEngineerCancelledSubTasks(string EngineerId);
    }

    
}
