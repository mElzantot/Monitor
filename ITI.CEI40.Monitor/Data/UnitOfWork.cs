using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data.Repositories.Managers;


namespace ITI.CEI40.Monitor.Data
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Claims = new ClaimManager(context);
            Departments = new DepartmentManager(context);
            DepartmentProjects = new DepartmentProjectsManager(context);
            Engineers = new EngineerManager(context);
            Projects = new ProjectManager(context);
            SubTasks = new SubTaskManager(context);
            Tasks = new TaskManager(context);
            Teams = new TeamManager(context);
            TeamTasks = new TeamTasksManager(context);

        }
        public IClaimManager Claims { get; }
        public IDepartmentManager Departments { get; }
        public IDepartmentProjectsManager DepartmentProjects { get; }
        public IEngineerManager Engineers { get; }
        public IProjectManager Projects { get; }
        public ISubTaskManager SubTasks { get; }
        public ITaskManager Tasks { get; }
        public ITeamManager Teams { get; }
        public ITeamTasksManager TeamTasks { get; }



        public int Complete()
        {
            return context.SaveChanges();
        }

    }
}
