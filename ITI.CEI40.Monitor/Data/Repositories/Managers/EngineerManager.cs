using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class EngineerManager: Reposiotry<ApplicationDbContext,ApplicationUser>,IEngineerManager
    {
        public EngineerManager(ApplicationDbContext context):base(context)
        {
        }
        public ApplicationUser GetWithAttriutes(string id)
        {
            return set.Where(e => e.Id == id).Include(e => e.Team).ThenInclude(e=>e.Department)
                .FirstOrDefault();
        }

        public IEnumerable<ApplicationUser> GetAllWithAttributes()
        {
            return set.Include(e => e.Team).ThenInclude(t=>t.Department);
        }

        public IEnumerable<ApplicationUser> GetEngineersInsideTeam(int teamId)
        {
            return set.Where(e => e.FK_TeamID == teamId).Include(e=>e.Team).ThenInclude(e=>e.Department).ToList();
        }

        public IEnumerable<ApplicationUser> GetEngineersWithSubtasks(int teamId)
        {
            return set.Where(e => e.FK_TeamID == teamId).Include(t=>t.SubTasks);
        }

        public List<ApplicationUser> GetEngineersInsideTeamWithSubTasks(int teamId)
        {
            List<ApplicationUser> applicationUsers= set.Where(e => e.FK_TeamID == teamId).Include(e=>e.SubTasks)
                .Include(s=>s.SubTasks).Select(e=>new ApplicationUser{
                UserName=e.UserName,
                SubTasks=e.SubTasks.Where(s=>s.Status==Status.Completed && s.ActualEndDate.Value.Month==DateTime.Now.Month).ToList()
                }).ToList();
            return applicationUsers;
        }

    }
}
