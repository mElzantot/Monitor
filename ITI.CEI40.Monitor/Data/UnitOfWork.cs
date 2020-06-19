using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data.Repositories.Managers;
using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;

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
            //EngineerSubTasks = new EngineerSubTasksManager(context);
            SubTaskSessions = new SubTaskSessionManager(context);
            Projects = new ProjectManager(context);
            SubTasks = new SubTaskManager(context);
            Tasks = new TaskManager(context);
            Teams = new TeamManager(context);
            TeamTasks = new TeamTasksManager(context);
            Invoices = new InvoicesManager(context);
            InoviceItems = new InvoiceItemManager(context);
            Dependency = new DependencyManager(context);
            Notification = new NotificationManager(context);
            NotificationUsers = new NotificationUsersManager(context);
            Files = new FilesManager(context);
            Comments = new CommentManager(context);
            Holiday = new HolidayManager(context);

        }
        public IClaimManager Claims { get; }
        public IDepartmentManager Departments { get; }
        public IDepartmentProjectsManager DepartmentProjects { get; }
        public IEngineerManager Engineers { get; }
        //public IEngineerSubTasksManager EngineerSubTasks { get; }
        public ISubTaskSessionManager SubTaskSessions { get; }
        public IProjectManager Projects { get; }
        public ISubTaskManager SubTasks { get; }
        public ITaskManager Tasks { get; }
        public ITeamManager Teams { get; }
        public ITeamTasksManager TeamTasks { get; }
        public IInvoicesManager Invoices { get; }
        public IInoviceItemManger InoviceItems { get; }
        public IDependencyManager Dependency { get; }
        public INotificationManager Notification { get;}
        public INotificationUsersManager NotificationUsers { get; }
        public IFilesManager Files { get; }
        public ICommentManager Comments { get; }
        public IHolidayManager Holiday { get; }




        public int Complete()
        {
            return context.SaveChanges();
        }

    }
}
