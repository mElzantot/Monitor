using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface ITeamManager : IRepository<Team>
    {
        Team FindByName(string name);
        Team GetTeamWithEngineersandLeader(int id);
        List<Team> getTeamsinsideDept(int deptID);
        Team GetTeamWithAttributes(int id);
        IEnumerable<Team> GetAllWithAttributes(int id);
        Team GetTeamWithTasksAndEngineers(int id);
        Team GetTeamWithTeamLeaderId(string teamLeaderId);
        Team GetTeamWithSubtasksAndEngineers(int id);
    }
}
