using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces
{
    public interface INotificationUsersManager :IRepository<NotificationUsers>
    {
        IEnumerable<NotificationUsers> GetNotificationsByUserId(string userId);
    }
}
