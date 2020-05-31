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


        public IEnumerable<Project> GetCompletedProjects()
        {
            return set.Where(pr =>  pr.Status == Status.Completed);
        }

        public IEnumerable<Project> GetCancelledProjects( )
        {
            return set.Where(pr => pr.Status == Status.Cancelled);
        }

        
        public IEnumerable<Project> GetAllProjects()
        {
            return set.Include(p => p.Status).Include(p => p.Task);
        }

        public Project GetProjectWithTasks(int projectId)
        {
            return set.Include(p => p.Task).FirstOrDefault(p => p.ID == projectId);

        }
        public IEnumerable<Project> GetRunningProjects()
        {
            return set.Where(pr => pr.Status != Status.Cancelled && pr.Status != Status.Completed);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return set.Include(p => p.Tasks).ToList();
        }

        public Project GetProjectForReport(int Projid)
        {
            return set.Where(pr => pr.ID == Projid).Include(pr => pr.ProjectManager).Include(pr => pr.Tasks).FirstOrDefault();
        }


    }
}
