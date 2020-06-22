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

        public DepManagerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeamsView()
        {
            Department Dep = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User));
            IEnumerable<Team> teams = unitOfWork.Teams.getTeamsinsideDept(Dep.Id);
            return View(teams);
        }

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            var tasks = unitOfWork.Tasks.GetHoldActiveTasks(teamId);
            return PartialView("_TaskPartialView", tasks);
        }

        [HttpGet]
        public IActionResult AssignTasks()
        {
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;
            var activityVM = new ActivityViewModel();
            activityVM.Tasks = unitOfWork.Tasks.GetDepartmentTasks(DepId).ToList();
            activityVM.Teams = unitOfWork.Teams.getTeamsinsideDept(DepId).ToList();

            return View("AssignTasks", activityVM);
        }


        [HttpGet]
        public PartialViewResult GetTask(int taskId)
        {
            ActDetailsViewModel ActDetailsVM = new ActDetailsViewModel
            {
                Task = unitOfWork.Tasks.GetTaskWithProjectAndTeam(taskId),
                HighComments = unitOfWork.Comments.GetHighCommentforTask(taskId).ToList(),
                MedComments = unitOfWork.Comments.GetMedCommentforTask(taskId).ToList()

            };
            return PartialView("_TaskPartial", ActDetailsVM);
        }


        [HttpPost]
        public JsonResult AssignTasks(int taskId, int teamId)
        {
            if (ModelState.IsValid)
            {
                var task = unitOfWork.Tasks.GetById(taskId);
                task.FK_TeamId = teamId;
                var team = unitOfWork.Teams.GetById(teamId);
                unitOfWork.Complete();

                //--------Add Notification to DataBase

                string messege = $"*{HttpContext.User.Identity.Name}=* -Department Manager- has assigned new task" +
                    $" *{task.Name}=* to your Team at *{DateTime.Now}=*";

                Notification Notification = new Notification
                {
                    messege = messege,
                    seen = false
                };
                Notification Savednotification = unitOfWork.Notification.Add(Notification);
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = team.FK_TeamLeaderId
                };
                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Team
                hubContext.Clients.User(notificationUsers.userID).SendAsync("newNotification", messege, false, Savednotification.Id);


                return Json(new { teamName = team.Name });
            }

            return Json(new { });
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

        public void ChangeStatus(int Id, int Status)
        {
            var task = unitOfWork.Tasks.GetById(Id);
            task.Status = (Status)Status;
            if (Status == 2)
            {
                task.ActualEndDate = DateTime.Now;
                #region notification
                int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
                Team team = unitOfWork.Teams.GetById(teamID);
                ApplicationUser teammanager = userManager.Users.FirstOrDefault(u => u.Id == team.FK_TeamLeaderId);
                Department dep = unitOfWork.Departments.GetById(team.FK_DepartmentId);
                string messege = $"Congartulations *{teammanager.UserName}=*  the task *{task.Name}=*  at *{DateTime.Now}=*.";
                SendNotification(messege, team.FK_TeamLeaderId);
                #endregion

            }
            else
            {

            }
            unitOfWork.Tasks.Edit(task);
        }

        [HttpGet]
        public IActionResult CancelledTasks(int depid)
        {
            var tasks = unitOfWork.Tasks.GetDepCancelledTasks(depid).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult ArchivedTasks()
        {
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;
            var tasks = unitOfWork.Tasks.Archive(DepId).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            var subtask = unitOfWork.SubTasks.GetSubTasksFromTask(taskId);
            return View("_DashBoardPartial", subtask);
        }

        public void SendNotification(string messege, params string[] usersId)
        {
            Notification Notification = new Notification
            {
                messege = messege,
                seen = false
            };
            Notification Savednotification = unitOfWork.Notification.Add(Notification);

            for (int i = 0; i < usersId.Length; i++)
            {
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = usersId[i]
                };
                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Employee
                hubContext.Clients.User(usersId[i]).SendAsync("newNotification", messege, false, Savednotification.Id);
            }

        }


    }
}