using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class DepartmentManager:Reposiotry<ApplicationDbContext,Department>,IDepartmentManager
    {
        public DepartmentManager(ApplicationDbContext context):base(context)
        {

        }

        public List<Department> GetDeptWithTeamsandEmployees()
        {
            return set.Include(e => e.Teams).ToList();
        }

        public Department  GetDeptWithManager(int id)
        {
            return set.Where(d => d.Id == id).Include(d => d.Manager).FirstOrDefault();
        }


        public Department FindByName(string name)
        {
            return set.Where(d => d.Name == name).FirstOrDefault();
        }


    }
}
