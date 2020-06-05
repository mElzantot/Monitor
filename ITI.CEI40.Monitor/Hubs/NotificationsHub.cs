using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Hubs
{
    public class NotificationsHub : Hub
    {



        public void sendMessages(string message)
        {

            Clients.All.SendAsync("newMessage", message);
        }

        public override Task OnConnectedAsync()
        {


            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }








    }
}
