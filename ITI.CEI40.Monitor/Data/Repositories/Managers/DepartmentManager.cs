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



    }
}
