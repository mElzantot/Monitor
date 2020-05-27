using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class ProjectManager:Reposiotry<ApplicationDbContext,Project>,IProjectManager
    {
        public ProjectManager(ApplicationDbContext context):base(context)
        {

        }

        public IEnumerable<Project> GetSearchName(string name)
        {
            return set.Where(pr => pr.Name.Contains(name)); 
        }

        
        public IEnumerable<Project> GetAllProjects()
        {
            return set.Include(p => p.Status).Include(p => p.Task);
        }

        
    }
}
