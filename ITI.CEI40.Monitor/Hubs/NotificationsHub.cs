using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace ITI.CEI40.Monitor.Hubs
{
    public class NotificationsHub : Hub 
    {
        public void sendNotification(string userid, string notification)
        {
            Clients.User(userid).SendAsync("newNotification", notification);
            //Clients.All.SendAsync("newNotification", notification);
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            return base.OnConnectedAsync();
        }

    }
}
