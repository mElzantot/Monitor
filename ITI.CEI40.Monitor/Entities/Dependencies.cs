using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Dependencies
    {
        public Dependencies(int lag = 0)
        {
            this.Lag = lag;
        }

        public int Lag { get; set; }

        public int ActivityToFollowId { get; set; }
        public Activity ActivityToFollow { set; get; }

        public int FollowingActivityId { get; set; }
        public Activity FollowingActivity { set; get; }
    }
}
