using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Hubs
{
    public  class NotificationsHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsHub(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public void sendNotification(string notification)
        {
            Clients.All.SendAsync("newNotification", notification);
            //Clients.All.SendAsync("newNotification", notification);
        }

        public override Task OnConnectedAsync()
        {
            userManager.GetUserId(Context.User);

            return base.OnConnectedAsync(); 
        }






    }
}
