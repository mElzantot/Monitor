using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public interface IDepartmentManager:IRepository<Department>
    {
        List<Department> GetAllDeptWithTeamsandManagers();

        Department GetDeptWithManager(int id);

        Department FindByName(string name);

        Department GetDeptWithTeamsandManager(int id);

         Department GetDeptWithTeamsAndProjects(int id);

        IEnumerable<Department> GetAllDeptWithManagers();

        Department GetDepartmentWithManagerID(string managerId);



    }

}
