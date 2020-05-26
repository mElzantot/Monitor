using ITI.CEI40.Monitor.Entities;
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


        public IEnumerable<Project> GetCompletedProjects()
        {
            return set.Where(pr =>  pr.Status == Status.Completed);
        }

        public IEnumerable<Project> GetCancelledProjects( )
        {
            return set.Where(pr => pr.Status == Status.Canceled);
        }

        public IEnumerable<Project> GetRunningProjects()
        {
            return set.Where(pr => pr.Status != Status.Canceled && pr.Status != Status.Completed);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return set.Include(p => p.Tasks).ToList();
        }


    }
}
