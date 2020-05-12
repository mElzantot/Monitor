using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class ClaimManager:Reposiotry<ApplicationDbContext,Claim>,IClaimManager
    {

        public ClaimManager(ApplicationDbContext context):base(context)
        {

        }
        
        public IEnumerable<Claim> GetAllWithAttributes()
        {
            return set.Include(c => c.SubTask);
        }
    }
}
