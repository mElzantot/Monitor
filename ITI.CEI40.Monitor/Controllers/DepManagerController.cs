using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models;
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

        public DepManagerController(IUnitOfWork unitOfWork , UserManager<ApplicationUser> userManager , IHubContext<NotificationsHub> hubContext)
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

            return View("/Views/Project/Details.cshtml",activityVM);
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

        [HttpGet]
        public IActionResult TeamsDashboard()
        {
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;
            var teams = unitOfWork.Teams.getTeamsinsideDept(DepId).ToList();

            List<string> names = new List<string>();
            List<List<float>> avg = new List<List<float>>();

            foreach (var item in teams)
            {
                names.Add(item.Name);
                if (item.Tasks!=null)
                {
                    avg.Add(TeamPerformence(item.Tasks.ToList()));
                }
                else
                {
                    avg.Add(null);
                }
            }
            TeamChartViewModel team = new TeamChartViewModel {
                Names=names,
                Values=avg
            };

            return View(team);
        }


        private List<float> TeamPerformence(List<Activity> task)
        {
            List<float> result = new List<float>() { 0, 0,0 };
            foreach (var item in task)
            {
                result[0] += item.ActualDuratoin;
                result[1] += item.Complexity;
                result[2] += 1;
                
            }

            return result;
        }

    }
}