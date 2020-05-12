using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface IDepartmentProjectsManager: IRepository<DepartmentProjects>
    {
        IEnumerable<DepartmentProjects> GetProjectWithDeptId(int Deptid);

        void RemoveDepProject(int projectId);
    }
}
