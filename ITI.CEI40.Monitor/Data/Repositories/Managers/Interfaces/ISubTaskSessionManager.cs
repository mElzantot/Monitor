﻿using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface ISubTaskSessionManager : IRepository<SubTaskSession>
    {
        IEnumerable<SubTaskSession> GetTimeSheetForEmp(string empId);
        IEnumerable<SubTaskSession> GetReportBySpecificDate(int teamId, DateTime date);


    }
}
