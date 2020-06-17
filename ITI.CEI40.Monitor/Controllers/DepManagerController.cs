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
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;

        public DepManagerController(IUnitOfWork unitofwork, UserManager<ApplicationUser> userManager, IHubContext<NotificationsHub> hubContext)
        {
            this.unitofwork = unitofwork;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeamsView(int DepId)
        {
            IEnumerable<Team> teams= unitofwork.Teams.getTeamsinsideDept(DepId);
            return View(teams);
        }

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            var task = unitofwork.Tasks.GetAllTaskWithTheirProject(teamId);
            return PartialView("_TaskPartialView", task);
        }

        [HttpGet]
        public IActionResult AssignTasks(int depid)
        {
            var activityVM = new ActivityViewModel();
            
            activityVM.Tasks = unitofwork.Tasks.GetDepartmentTasks(depid);

            activityVM.Teams = unitofwork.Teams.getTeamsinsideDept(depid).ToList();

            return View(activityVM);
        }

        

        [HttpPost]
        public JsonResult AssignTasks(int taskId,int teamId)
        {
            if (ModelState.IsValid)
            {
                var task = unitofwork.Tasks.GetById(taskId);
                task.FK_TeamId = teamId;
                var team = unitofwork.Teams.GetById(teamId);
                unitofwork.Complete();

                //--------Add Notification to DataBase

                string messege = $" New Task " +
                    $"{HttpContext.User.Identity.Name} -Department Manager- has assigned new task" +
                    $" -{task.Name}- to your Team at {DateTime.Now}";

                Notification Notification = new Notification
                {
                    messege = messege,
                    seen = false
                };
                Notification Savednotification = unitofwork.Notification.Add(Notification);
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = team.FK_TeamLeaderId
                };
                unitofwork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Team
                hubContext.Clients.User(notificationUsers.userID).SendAsync("newNotification", messege);


                return Json(new { teamName=team.Name});
            }
            return Json(new {  });
        }

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            var subtask = unitofwork.SubTasks.GetSubTasksFromTask(taskId);
            return View("_DashBoardPartial", subtask);
        }



    }
}