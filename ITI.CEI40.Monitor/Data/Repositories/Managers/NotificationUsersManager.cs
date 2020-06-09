using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class NotificationUsersManager : Reposiotry<ApplicationDbContext,NotificationUsers>,INotificationUsersManager
    {
        public NotificationUsersManager(ApplicationDbContext context) :base(context)
        {
        }

    }
}
