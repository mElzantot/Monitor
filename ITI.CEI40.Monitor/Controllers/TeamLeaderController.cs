using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TeamLeaderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;

        public TeamLeaderController(IUnitOfWork unitofwork, UserManager<ApplicationUser> userManager, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitofwork;
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        //Shows the list of engineers inside every team leader's team

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersView()
        {
            // to get the team id from the id of the logged in team leader
            int teamId = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;

            //to get the list of engineers inside of this team
            List<ApplicationUser> TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList();

            //to exclude the team leader of this list 
            TeamMembers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);


            SubTaskSubmitVM subTaskSubmitVM = new SubTaskSubmitVM
            {
                teamMembers = TeamMembers
            };
            return View(subTaskSubmitVM);
        }


        //Shows the sub tasks for each engineer

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult displaySubTasks(string engId)
        {

            //to get the list of sub tasks assigned to this engineer
            List<SubTask> SubTasks = unitOfWork.SubTasks.GetEngineerSubTasks(engId);

            return PartialView("_SubTaskPartialView", SubTasks);
        }


        //allows the team leader to cancel a sub task
        [Authorize(Roles = "TeamLeader")]
        public void CancelSubTask(int id)
        {
            //gets the sub task you want to cancel by its id
            var subtask = unitOfWork.SubTasks.GetById(id);

            //changes the status of the sub task to cancelled
            subtask.Status = Status.Cancelled;

            //apply changes to data base
            unitOfWork.SubTasks.Edit(subtask);

            //sends a notificatin to the engineer that his sub task is cancelled
            string messege = $"Your Team Leader has cancelled *{subtask.Name}=* at *{DateTime.Now}=*";
            SendNotification(messege, subtask.FK_EngineerID);
        }


        //allows the team leader to recieve a completed sub task and evaluates it
        [Authorize(Roles = "TeamLeader")]
        [HttpPost]
        public void SubmitSubTask(SubmitModal submitModal)
        {
            //gets the sub task that a certain engineer submitted
            var subtask = unitOfWork.SubTasks.GetSubTaskWithEngineer(submitModal.ID);

            //changes the status of the sub task to completed
            subtask.Status = Status.Completed;

            //evaluates the complexity measure for this sub task
            subtask.Complexity = submitModal.Complexity;

            //evaluates the time management measure for this sub task
            subtask.TimeManagement = submitModal.TimeManagement;

            //evaluates the quality measure for this sub task
            subtask.Quality = submitModal.Quality;

            //apply changes to data base
            unitOfWork.SubTasks.Edit(subtask);

            //sends a notificatin to the engineer that his sub task is successfully submitted
            string messege = $"Congratulations Your Team Leader Submitted *{subtask.Name}=* at *{DateTime.Now}=*";
            SendNotification(messege, subtask.FK_EngineerID);
        }


        //Shows the completed or cancelled sub task for each engineer
        [Authorize(Roles = "Engineer")]
        [HttpGet]
        public IActionResult ArchivedSubTasks()
        {

            //gets the logged in engineer id
            string engId = userManager.GetUserId(HttpContext.User);

            //gets this engineer completed or cancelled sub tasks
            var subtasks = unitOfWork.SubTasks.Archive(engId).ToList();

            return View("ArchivedSubTasks", subtasks);
        }



        //Shows the dashboard of each engineer's performance

        [Authorize(Roles = "Engineer")]
        public IActionResult EngineerChart()
        {
            //gets the logged in engineer id
            string engId = userManager.GetUserId(HttpContext.User);

            //get the list of completed sub tasks assigned to this engineer
            List<SubTask> subtasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engId);


            List<string> months = new List<string>();

            //list of the quality measure for this engineer
            List<float> quality = new List<float>();

            //list of the complexity measure for this engineer
            List<float> complexity = new List<float>();

            //list of the time measure for this engineer
            List<float> time = new List<float>();

            List<SubTask> subs = new List<SubTask>();
            string engName;
            if (subtasks.Count>0)
            {
                 engName = subtasks[0].Engineer.UserName;
            }
            else
            {
                engName = "";
            }
            string month;
            int i = -1;


            foreach (var item in subtasks)
            {

                //gets the sub tasks completed in this month
                month = item.ActualEndDate.Value.ToString("MMMM");
                if (!months.Contains(month))
                {
                    months.Add(month);
                    complexity.Add(item.Complexity);
                    quality.Add(item.Quality * item.Complexity);
                    time.Add(item.TimeManagement * item.Complexity);
                    i++;
                }
                else
                {
                    complexity[i] += item.Complexity;
                    quality[i] += item.Quality * item.Complexity;
                    time[i] += item.TimeManagement * item.Complexity;
                }
            }

            for (int j = 0; j < months.Count(); j++)
            {
                quality[j] = quality[j] / complexity[j];
                time[j] = time[j] / complexity[j];
            }

            foreach (var item in subtasks)
            {
                if (item.ActualEndDate.HasValue && item.ActualEndDate.Value.Month == DateTime.Now.Month)
                {
                    subs.Add(item);
                }
            }

            EngineerChrtViewModel engineerChrtView = new EngineerChrtViewModel
            {
                EngineerName = engName,
                Months = months,
                Quality = quality,
                Time = time,
                LastTasks = subs,
                Complexity = complexity
            };
            return View("EngineerChart", engineerChrtView);
        }



        //Shows the dashboard of the team leader's team engineers

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersChart()
        {

            //gets the team's id from the logged in team leader's id
            int teamId = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            

            //gets the list of engineers inside this team
            List<ApplicationUser> teamMenmbers = unitOfWork.Engineers.GetEngineersInsideTeamWithSubTasks(teamId);

            #region remove the DepManager
            Team team = unitOfWork.Teams.GetById(teamId);
            string depmanid = unitOfWork.Departments.GetDepManagerIdWithDepId(team.FK_DepartmentId);
            ApplicationUser depmanager = userManager.Users.FirstOrDefault(u => u.Id == depmanid);
            teamMenmbers.RemoveAll(e => e.UserName == depmanager.UserName);
            #endregion
            // remove the team leader from this list
            teamMenmbers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);

            //list of each engineer's id
            List<string> Ids = new List<string>();

            //list of the engineers names
            List<string> names = new List<string>();

            //list of engineers performance measures
            List<List<float>> avg = new List<List<float>>();

            foreach (var item in teamMenmbers)
            {
                names.Add(item.UserName);
                Ids.Add(item.Id);
                if (item.SubTasks != null)
                {
                    avg.Add(EngineerPerformence(item.SubTasks.ToList()));
                }
                else
                {
                    avg.Add(null);
                }
            }

            TeamChartViewModel teamChart = new TeamChartViewModel
            {
                Names = names,
                Values = avg,
                EngIds = Ids
            };
            return View("TeamChart", teamChart);
        }


        //function to return each engineer's performance measures

        private List<float> EngineerPerformence(List<SubTask> subTasks)
        {
            List<float> result = new List<float>() { 0, 0, 0, 0, 0 };
            foreach (var item in subTasks)
            {
                result[0] += item.ActualDuration;
                result[1] += item.Complexity;
                result[2] += item.Quality * item.Complexity;
                result[3] += item.TimeManagement * item.Complexity;
                result[4] += 1;
            }
            result[2] = result[2] / result[1];
            result[3] = result[3] / result[1];
            return result;
        }



        //shows the tasks assigned to the team leader's team

        public IActionResult MyTasks()
        {
            //gets the team id using the logged in team leader's id
            int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;

            //gets the tasks assigned to this team
            var tasks = unitOfWork.Tasks.GetTasksByTeamIDWithSubs(teamID);


            return View("MyTasks", tasks);
        }



        //Shows the tasks with all its details like comments

        public PartialViewResult MyTaskPartial(int taskId)
        {
            ActDetailsViewModel ActDetailsVM = new ActDetailsViewModel
            {
                Task = unitOfWork.Tasks.GetTaskWithProjectAndTeam(taskId),
                MedComments = unitOfWork.Comments.GetMedCommentforTask(taskId).ToList()
            };
            return PartialView("_MyTasksPartial", ActDetailsVM);
        }



        //Allows the team leader to add a new sub task

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult AddSubTask(int taskID)
        {
            var taskVM = new TaskViewModel
            {
                TaskId = taskID,
                TaskName = unitOfWork.Tasks.GetById(taskID).Name,
                taskDescription = unitOfWork.Tasks.GetById(taskID).Description,
                SubTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(taskID),
            };
            return View("AddSubTask", taskVM);
        }




        //shows the cancelled sub tasks assigned to a certain engineer

        [HttpGet]
        public IActionResult displayCancellesSubTasks(string engId)
        {

            //gets the list of cancelled sub tasks assigned to this engineer
            List<SubTask> subtasks = unitOfWork.SubTasks.GetEngineerSubTasks(engId);

            return PartialView(subtasks);
        }



        //displays the resources utilization chart

        [HttpGet]
        public IActionResult displayAll()
        {
            //gets the team using the logged in team leader's id
            Team team = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User));

            //gets the engineers inside this team
            List<ApplicationUser> Engineers = unitOfWork.Engineers.GetEngineersWithSubtasks(team.Id).ToList();

            //removes the team leader from this list
            Engineers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);
            ResourceChartVM resourceChartVM = new ResourceChartVM
            {
                Employees = Engineers,
            };

            return View(resourceChartVM);
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


        //Gets the approximate duration measure each engineer will spend doing each sub task

        public JsonResult GetApproximateDuration(string engineerId, int complexity, int Quality)
        {

            //gets the list of completed sub tasks assigned to this engineer 
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engineerId);

            SubTask newSubTask = new SubTask
            {
                Complexity = complexity,
                Quality = Quality
            };

            PredictedDuration predictedDuration = MLClass.PredictDurationBasedonQualityandCompl(subTasks, newSubTask);
            return Json(predictedDuration);
        }


        //Gets the approximate quality measure for  each engineer sub task

        public JsonResult GetApproximateQuaity(string engineerId, int complexity, int Duration)
        {

            //gets the list of completed sub tasks assigned to this engineer 
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engineerId);

            SubTask newSubTask = new SubTask
            {
                Complexity = complexity,
                ActualDuration = Duration
            };

            PredictedQuality predictedQuality = MLClass.PredictQualityBasedonDurationandCompl(subTasks, newSubTask);
            return Json(predictedQuality);

        }



    }
}