using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class TaskManager:Reposiotry<ApplicationDbContext,Activity>,ITaskManager
    {
        public TaskManager(ApplicationDbContext context):base(context)
        {

        }
        public IEnumerable<Activity> GetAllWithAttributes()
        {
            return set.Include(t => t.Project).Include(t => t.Team);
        }
        

        public IEnumerable<Activity> GetTasksByTeamID(int teamId)
        {
            return set.Where(t => t.FK_TeamId == teamId).Include(t => t.Project).ToList();
        }


        public Activity GetTaskWithProject(int taskId)
        {
            return set.Where(t => t.Id == taskId).Include(t => t.Project).FirstOrDefault();
        }



        public IEnumerable<Activity> GetAllTaskWithTheirProject(int teamId)
        {
            return set.Where(t => t.FK_TeamId == teamId).Include(t => t.Project).Include(t=>t.Team).ToList();
        }


        public IEnumerable<Activity> GetActivitiesFromProject(int projId)
        {
            return set.Where(p => p.FK_ProjectId == projId).Include(a => a.Project)
                .Include(p => p.SubTasks);
        }

        public IEnumerable<Activity> GetDepartmentTasks(int depid)
        {
            return set.Where(a => a.FK_DepID == depid).Include(a => a.Team).Include(a=>a.Project).ToList();
        }
        public IEnumerable<Activity> GetByProjectId(int id)
        {
            return set.Where(t => t.FK_ProjectId == id).Where(t => t.Status != Status.Cancelled).Include(t=>t.FollowingActivities).Include(t => t.ActivitiesToFollow);
        }
        public IEnumerable<Activity> GetByProIdAndViewOrder(int proId, int viewOrder)
        {
            return set.Where(t => t.FK_ProjectId == proId).Where(t => t.ViewOrder == viewOrder);
        }

    }
}
