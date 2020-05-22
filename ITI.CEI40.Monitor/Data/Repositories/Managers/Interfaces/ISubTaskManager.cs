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
        IEnumerable<SubTask> GetEngineerSubTasksFromTask(int taskId);

        IEnumerable<SubTask> GetSubTasksByEngineerId(string engineerId);


    }

    
}
