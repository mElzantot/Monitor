using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface IEngineerManager: IRepository<ApplicationUser>
    {
        ApplicationUser GetWithAttriutes(string id);
        IEnumerable<ApplicationUser> GetAllWithAttributes();
        IEnumerable<ApplicationUser> GetEngineersInsideTeam(int id);
        List<ApplicationUser> GetEngineersInsideTeamWithSubTasks(int teamId);
        IEnumerable<ApplicationUser> GetEngineersWithSubtasks(int id);

    }

}
