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

        public IEnumerable<SubTaskSession> GetTimeSheetForEmp(string empId)
        {
            return set.Where(st => st.EmpId == empId).Include(st => st.SubTask).ToList();
        }

        public IEnumerable<SubTaskSession> GetReportBySpecificDate(int teamId  ,DateTime date)
        {
            return set.Where(st => st.SessStartDate.Date == date).Include(e=>e.Employee);
        }

    }
}
