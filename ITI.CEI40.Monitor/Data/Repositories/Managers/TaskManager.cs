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
        public IEnumerable<Activity> GetByProjectId(int id)
        {
            return set.Where(t => t.FK_ProjectId == id);
        }
    }
}
