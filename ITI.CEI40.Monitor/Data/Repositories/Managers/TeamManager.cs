using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class TeamManager : Reposiotry<ApplicationDbContext, Team>, ITeamManager
    {
        public TeamManager(ApplicationDbContext context) : base(context)
        {

        }
        
        public List<Team> getTeamsinsideDept(int deptID)
        {
            return set.Where(e => e.FK_DepartmentId == deptID).Include(t => t.TeamLeader)
                .Include(t=>t.Engineers).Include(t => t.Tasks).ToList();
        }

        public Team GetTeamWithAttributes(int id)
        {
            return set.Where(t => t.Id == id).Include(t => t.Department).Include(t => t.TeamLeader)
               .Include(t => t.Engineers).Include(t => t.Tasks).FirstOrDefault();
        }


        public IEnumerable<Team> GetAllWithAttributes(int id)
        {
            return set.Where(t => t.FK_DepartmentId == id).Include(t => t.Department).Include(t => t.TeamLeader)
               .Include(t => t.Engineers).Include(t => t.Tasks);
        }

        public Team GetTeamWithTasksAndEngineers(int id)
        {
            return set.Where(t => t.Id == id).Include(t => t.Engineers).Include(t => t.Tasks).FirstOrDefault();
        }

        public Team FindByName(string name)
        {
            return set.Where(t => t.Name == name).FirstOrDefault();
        }

        public Team GetTeamWithEngineersandLeader(int id)
        {
            return set.Where(t => t.Id == id).Include(t => t.TeamLeader).Include(t => t.Engineers).FirstOrDefault();
        }



        public Team GetTeamWithTeamLeaderId(string teamLeaderId)
        {
            return set.Where(t => t.FK_TeamLeaderId == teamLeaderId).FirstOrDefault();
        }

        public Team GetTeamWithSubtasksAndEngineers(int id)
        {
            return set.Where(t => t.Id == id).Include(t => t.Engineers)
                .ThenInclude(e => e.SubTasks).FirstOrDefault();
        }
    }
}
