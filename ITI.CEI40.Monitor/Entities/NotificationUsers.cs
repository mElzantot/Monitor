using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class NotificationUsers
    {

        public ApplicationUser User { get; set; }

        public string userID { get; set; }

        public Notification Notification { get; set; }

        public int NotificationId { get; set; }

    }
}
