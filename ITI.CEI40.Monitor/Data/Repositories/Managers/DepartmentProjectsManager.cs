using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class DepartmentProjectsManager:Reposiotry<ApplicationDbContext,DepartmentProjects>,IDepartmentProjectsManager
    {
        public DepartmentProjectsManager(ApplicationDbContext context):base(context)
        {

        }

        public IEnumerable<DepartmentProjects> GetProjectWithDeptId(int Deptid)
        {
            return set.Where(d => d.DepartmentID == Deptid).Include(d => d.Project);
        }

        public void RemoveDepProject(int projectId)
        {
            set.RemoveRange(set.Where(d => d.ProjectID == projectId));
        }
    }
}
