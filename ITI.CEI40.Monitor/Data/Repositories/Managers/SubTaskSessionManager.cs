using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class SubTaskSessionManager : Reposiotry<ApplicationDbContext, SubTaskSession>, ISubTaskSessionManager
    {
        public SubTaskSessionManager(ApplicationDbContext context) : base(context)
        {


        }
        public SubTaskSession GetLastSessBySubTaskID(int subTaskId)
        {
            return set.Where(se => se.FK_SubTaskID == subTaskId).LastOrDefault();
        }

        public IEnumerable<SubTaskSession> GetTimeSheetForEmp(string empId)
        {
            return set.Where(st => st.EmpId == empId && st.SessEndtDate != null).Include(st => st.SubTask).ToList();
        }

        public IEnumerable<SubTaskSession> GetReportBySpecificDate(int teamId  ,DateTime date)
        {
            return set.Where(st => st.SessStartDate.Date == date).Include(e=>e.Employee);
        }

        public SubTaskSession GetOpenSubTask(string empId)
        {
            return set.Where(st => st.EmpId == empId && st.SessEndtDate == null).Include(se=>se.SubTask).FirstOrDefault();
        }


    }
}
