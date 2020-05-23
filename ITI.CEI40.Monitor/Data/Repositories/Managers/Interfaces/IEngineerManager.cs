using ITI.CEI40.Monitor.Entities;
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

        IEnumerable<ApplicationUser> GetEngineers(int teamId);
    }

}
