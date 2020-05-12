﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data.Repositories.Managers;

namespace ITI.CEI40.Monitor.Data
{
    public interface IUnitOfWork
    {
        IClaimManager Claims { get; }
        IDepartmentManager Departments { get; }
        IDepartmentProjectsManager DepartmentProjects { get; }
        IEngineerManager Engineers { get; }
        IEngineerSubTasksManager EngineerSubTasks { get; }
        IProjectManager Projects { get; }
        ISubTaskManager SubTasks { get; }
        ITaskManager Tasks { get; }
        ITeamManager Teams { get; }
        ITeamTasksManager TeamTasks { get; }

        int Complete();

    }
}