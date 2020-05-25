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
        private readonly UserManager<ApplicationUser> userManager;

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

        public IEnumerable<ApplicationUser> GetEngineersInsideTeam(int id)
        {
            //var teamLeader = await userManager.GetUsersInRoleAsync("Team Leader");        
            return set.Where(e => e.FK_TeamID == id).ToList();
        }

    }
}
