using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class FilesManager : Reposiotry<ApplicationDbContext, Files>, IFilesManager
    {
        public FilesManager(ApplicationDbContext context) : base(context)
        {

        }
    }
}