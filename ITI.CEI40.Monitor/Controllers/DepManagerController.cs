using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class DepManagerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;

        public DepManagerController(IUnitOfWork unitofwork , UserManager<ApplicationUser> userManager , IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitofwork;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeamsView(int DepId)
        {
            IEnumerable<Team> teams= unitOfWork.Teams.getTeamsinsideDept(DepId);
            return View(teams);
        }

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            var tasks = unitOfWork.Tasks.GetHoldActiveTasks(teamId);
            return PartialView("_TaskPartialView", tasks);
        }

        [HttpGet]
        public IActionResult AssignTasks(int depid)
        {
            var activityVM = new ActivityViewModel();
            
            activityVM.Tasks = unitOfWork.Tasks.GetDepartmentTasks(depid);

            activityVM.Teams = unitOfWork.Teams.getTeamsinsideDept(depid).ToList();

            return View(activityVM);
        }



        [HttpPost]
        public  JsonResult AssignTasks(int taskId,int teamId)
        {
            if (ModelState.IsValid)
            {
                var task = unitOfWork.Tasks.GetById(taskId);
                task.FK_TeamId = teamId;
                var team = unitOfWork.Teams.GetById(teamId);
                unitOfWork.Complete();

                //--------Add Notification to DataBase

                Notification Notification = new Notification
                {
                    Action = Entities.Enums.NotificationType.Assigned,
                    TaskName = task.Name,
                    FK_SenderId = userManager.GetUserId(HttpContext.User),
                    NotifTime = DateTime.Now
                };

                Notification Savednotification = unitOfWork.Notification.Add(Notification);
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = team.FK_TeamLeaderId
                };

                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Team

                string message = $" New Task " +
                    $"{HttpContext.User.Identity.Name} -Department Manager- has assigned new task" +
                    $" -{Notification.TaskName}- to your Team at {Notification.NotifTime}";
                hubContext.Clients.User(notificationUsers.userID).SendAsync("newNotification", message);


                return Json(new { teamName=team.Name});
            }

            return Json(new {  });
        }

        //Omar to edit status if the Departement manager submit the subtask
        public void EditStatus(int id, int status)
        {
            var task = unitOfWork.Tasks.GetById(id);
            task.Status = (Status)status;
            if (task.Status == Status.Cancelled)
            {
                List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(task.Id);
                foreach (var item in subTasks)
                {
                    item.Status = Status.Cancelled;
                    //Want to Edit // Make Edit for List of items
                    unitOfWork.SubTasks.Edit(item);
                }
            }
            unitOfWork.Tasks.Edit(task);
        }



        [HttpGet]
        public IActionResult CancelledTasks(int depid)
        {
            var tasks = unitOfWork.Tasks.GetDepCancelledTasks(depid).ToList();
            return View( tasks);
        }

        
    }
}