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
    public class NotificationsHub : Hub
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsHub(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public void sendNotification(string userid, string notification)
        {
            Clients.User(userid).SendAsync("newNotification", notification);
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            List<NotificationUsers> NotificationUsers = unitOfWork.NotificationUsers.GetNotificationsByUserId(userId).OrderByDescending(n => n.NotificationId).Take(3).Reverse().ToList();
            foreach (NotificationUsers notificationuser in NotificationUsers)
            {
                Clients.User(userId).SendAsync("newNotification", notificationuser.Notification.messege, notificationuser.Notification.seen, notificationuser.Notification.Id);
            }
            return base.OnConnectedAsync();
        }


    }
}
