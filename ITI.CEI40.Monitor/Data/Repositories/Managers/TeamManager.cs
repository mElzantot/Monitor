using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class TeamManager:Reposiotry<ApplicationDbContext,Team>,ITeamManager
    {
        public TeamManager(ApplicationDbContext context):base(context)
        {

        }

        public List<Team> getTeamsinsideDept(int deptID)
        {
            return set.Where(e => e.FK_DepartmentId == deptID).Include(e=>e.Tasks).Include(t=>t.TeamLeader).ToList();
        }


        public Team GetTeamWithAttributes(int id)
        {
            return set.Where(t => t.Id == id).Include(t => t.Department).Include(t => t.TeamLeader)
               .Include(t=>t.Engineers).Include(t=>t.Tasks).FirstOrDefault();
        }

        public IEnumerable<Team> GetAllWithAttributes()
        {
            return set.Include(t => t.Department).Include(t => t.TeamLeader)
               .Include(t => t.Engineers).Include(t => t.Tasks);
        }
    }
}
