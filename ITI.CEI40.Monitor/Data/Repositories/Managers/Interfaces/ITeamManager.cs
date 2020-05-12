﻿using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface ITeamManager: IRepository<Team>
    {
        List<Team> getTeamsinsideDept(int deptID);

        Team GetTeamWithAttributes(int id);
        IEnumerable<Team> GetAllWithAttributes();
    }
    
}