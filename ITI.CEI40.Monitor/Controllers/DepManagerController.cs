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

        //Shows the list of teams inside every department managers's department

        public IActionResult TeamsView()
        {

            // to get the dep id from the id of the logged in department manager
            Department Dep = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User));

            // to get the list of teams inside this department
            IEnumerable<Team> teams = unitOfWork.Teams.getTeamsinsideDept(Dep.Id);

            return View(teams);
        }


        //Shows the tasks assigned to each team inside the department

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            //gets the the list of tasks assigned to a team using the team id
            var tasks = unitOfWork.Tasks.GetHoldActiveTasks(teamId);

            return PartialView("_TaskPartialView", tasks);
        }


        //Allows the department manager to assign tasks to a certain team

        [HttpGet]
        public IActionResult AssignTasks()
        {
            //gets the department id using the logged in department manager id
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;

            var activityVM = new ActivityViewModel();

            //gets the task assigned to this department
            activityVM.Tasks = unitOfWork.Tasks.GetDepartmentTasksIsCompleted(DepId).ToList();

            //gets the teams inside this department
            activityVM.Teams = unitOfWork.Teams.getTeamsinsideDept(DepId).ToList();

            return View("AssignTasks", activityVM);
        }


        //gets each task with its comments from the comments hub

        [HttpGet]
        public PartialViewResult GetTask(int taskId)
        {
            ActDetailsViewModel ActDetailsVM = new ActDetailsViewModel
            {
                //gets the task from database by its id
                Task = unitOfWork.Tasks.GetTaskWithProjectAndTeam(taskId),

                //gets this task comments 
                HighComments = unitOfWork.Comments.GetHighCommentforTask(taskId).ToList(),
                MedComments = unitOfWork.Comments.GetMedCommentforTask(taskId).ToList()
            };
            return PartialView("_TaskPartial", ActDetailsVM);
        }


        //Allows the department manager to assign tasks to a certain team (post)

        [HttpPost]
        public JsonResult AssignTasks(int taskId, int teamId)
        {
            if (ModelState.IsValid)
            {

                //gets the task you want to assign from data base
                var task = unitOfWork.Tasks.GetById(taskId);

                //assignes it to a certain team inside the department
                task.FK_TeamId = teamId;
                var team = unitOfWork.Teams.GetById(teamId);

                //apply changes to data base
                unitOfWork.Complete();

                //sends a notification to the team leader that this task is assigned to his team
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

                hubContext.Clients.User(notificationUsers.userID).SendAsync("newNotification", messege, false, Savednotification.Id);


                return Json(new { teamName = team.Name });
            }

            return Json(new { });
        }


        //to edit the status if the Departement manager submit the task
        public void EditStatus(int id, int status)
        {
            //gets this task by its id
            var task = unitOfWork.Tasks.GetById(id);

            //changes the status of this task
            task.Status = (Status)status;

            //chech if the new task status is cancelled
            if (task.Status == Status.Cancelled)
            {
                //gets every sub task inside this task
                List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(task.Id);

                //cancelles each one of those sub tasks 
                foreach (var item in subTasks)
                {
                    item.Status = Status.Cancelled;
                    unitOfWork.SubTasks.Edit(item);
                }
            }
            //apply changes to data base
            unitOfWork.Tasks.Edit(task);
        }

        //function that changes the status of a certain task

        public void ChangeStatus(int Id, int Status)
        {

            //gets the task from data base by its id
            var task = unitOfWork.Tasks.GetTaskWithTeamLeader(Id);

            //change the status of the task
            task.Status = (Status)Status;

            //chech if the task status is cancelled
            if (Status == 2)
            {
                task.ActualEndDate = DateTime.Now;
                task.IsCompleted = true;


                //sends a notification to the team leader that the task is completed
                #region notification
                ApplicationUser teammanager = task.Team.TeamLeader;
                string messege = $"Congartulations *{teammanager.UserName}=*  the task *{task.Name}=*  at *{DateTime.Now}=*.";
                SendNotification(messege,teammanager.Id);
                #endregion
            }
            else
            {
                //sends a notification to the team leader that the task is cancelled
                #region notification
                ApplicationUser teammanager = task.Team.TeamLeader;
                string messege = $"Your Departement Manager has cancelled this task : *{task.Name}=*  at *{DateTime.Now}=*.";
                SendNotification(messege, teammanager.Id);
                #endregion
            }
            unitOfWork.Tasks.Edit(task);
        }


        //gets the cancelled tasks inside a certain department

        [HttpGet]
        public IActionResult CancelledTasks(int depid)
        {
            //gets the list of cancelled tasks assigned to this department
            var tasks = unitOfWork.Tasks.GetDepCancelledTasks(depid).ToList();

            return View(tasks);
        }


        //Shows the completed or cancelled task for each department

        [HttpGet]
        public IActionResult ArchivedTasks()
        {

            //gets the department id using the logged in department manager id
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;

            //gets this department completed or cancelled tasks
            var tasks = unitOfWork.Tasks.Archive(DepId).ToList();

            return View(tasks);
        }


        //Shows the dashboard of each task

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            //gets the list of sub tasks inside a task by its id
            var subtask = unitOfWork.SubTasks.GetSubTasksFromTask(taskId);

            return View("_DashBoardPartial", subtask);
        }



        //function to send notifications to a certain user

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
        

        //Shows the dashboard of each department's teams performance 

        [HttpGet]
        public IActionResult TeamsDashboard()
        {

            //gets the department id using the logged in department manager id
            int DepId = unitOfWork.Departments.GetDepartmentWithManagerID(userManager.GetUserId(HttpContext.User)).Id;

            //gets the teams inside this department
            var teams = unitOfWork.Teams.getTeamsinsideDept(DepId).ToList();


            //list of teams names
            List<string> names = new List<string>();

            //list of teams performance measures
            List<List<float>> avg = new List<List<float>>();

            foreach (var item in teams)
            {
                names.Add(item.Name);
                if (item.Tasks != null)
                {
                    avg.Add(TeamPerformence(item.Tasks.ToList()));
                }
                else
                {
                    avg.Add(null);
                }
            }
            TeamChartViewModel team = new TeamChartViewModel
            {
                Names = names,
                Values = avg
            };

            return View(team);
        }



        //function to return each team performance measures

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