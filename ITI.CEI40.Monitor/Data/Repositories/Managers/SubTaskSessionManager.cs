using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class SubTaskSessionManager:Reposiotry<ApplicationDbContext, SubTaskSession>, ISubTaskSessionManager
    {
        public SubTaskSessionManager(ApplicationDbContext context) : base(context)
        {


        }
        public SubTaskSession GetLastSessBySubTaskID(int subTaskId)
        {
            return set.Where(se => se.FK_SubTaskID == subTaskId).LastOrDefault();
        }
    }
}
