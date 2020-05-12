using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface IEngineerSubTasksManager : IRepository<EngineerSubTasks>
    {
        void RemoveEngineerSubTask(int stId);
        IEnumerable<EngineerSubTasks> GetEngineerswithsubtask(int SubTaskId);

        IEnumerable<EngineerSubTasks> GetSubTasksFromEngineerId(string EngId);



    }
}
