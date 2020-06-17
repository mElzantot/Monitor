using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;
        private static int Step = 3;

        public NotificationController(IUnitOfWork unitOfWork, IHubContext<NotificationsHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool SeeMessege(int id)
        {
            Notification notification = unitOfWork.Notification.GetById(id);
            if (notification != null)
            {
                notification.seen = true;
                unitOfWork.Notification.Edit(notification);
                return true;
            }
            return false;
        }

        public JsonResult LoadMore(int c)
        {
            string userId = userManager.GetUserId(User);
            if (userId != null)
            {
                List<NotificationUsers> NotificationUsers = unitOfWork.NotificationUsers.GetNotificationsByUserId(userId).OrderByDescending(n => n.NotificationId).Skip(c).Take(3).ToList();
                Step += 3;
                return Json(NotificationUsers);
            }
            else { return null; }
        }
    }
}