using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface IProjectManager : IRepository<Project>
    {
        IEnumerable<Project> GetSearchName(string name);
        IEnumerable<Project> GetAllProjects();
        Project GetProjectWithTasks(int projectId);
        IEnumerable<Project> GetCompletedProjects();
        IEnumerable<Project> GetCancelledProjects();
        IEnumerable<Project> GetRunningProjects();
        Project GetProjectForReport(int Projid);


    }
}
