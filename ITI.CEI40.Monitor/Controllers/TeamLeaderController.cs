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

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersView()
        {
            int teamId = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            List<ApplicationUser> TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList();
            TeamMembers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);
            return View(TeamMembers);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult displaySubTasks(string engId)
        {
            List<SubTask> subtask = unitOfWork.SubTasks.GetEngineerSubTasks(engId);
            return PartialView("_SubTaskPartialView", subtask);
        }

        [Authorize(Roles = "TeamLeader")]
        //Omar 
        public void CancelSubTask(int id)
        {
            var subtask = unitOfWork.SubTasks.GetById(id);
            subtask.Status = Status.Cancelled;
            unitOfWork.SubTasks.Edit(subtask);

            // notification
            string messege = $"Your Team Leader has cancelled *{subtask.Name}=* at *{DateTime.Now}=*";
            SendNotification(messege, subtask.FK_EngineerID);
        }

        [Authorize(Roles = "TeamLeader")]
        //Omar 
        [HttpPost]
        public void SubmitSubTask(SubTask subTask)
        {
            //var subtask = unitofwork.SubTasks.GetSubTaskWithEngineer(id);
            //subtask.Status = Status.Completed;
            //subtask.Complexity = complexity;
            //subtask.TimeManagement = time;
            //subtask.Quality = quality;
            //subtask.Engineer.TotalEvaluation;
            //unitofwork.SubTasks.Edit(subtask);  
        }

        [Authorize(Roles = "Engineer")]
        [HttpGet]
        public IActionResult ArchivedSubTasks()
        {
            string engId = userManager.GetUserId(HttpContext.User);
            var subtasks = unitOfWork.SubTasks.Archive(engId).ToList();
            return View("ArchivedSubTasks", subtasks);
        }

        //omar
        [Authorize(Roles = "Engineer")]
        public IActionResult EngineerChart()
        {

            string engId = userManager.GetUserId(HttpContext.User);
            List<SubTask> subtasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engId);


            List<string> months = new List<string>();
            List<float> quality = new List<float>();
            List<float> complexity = new List<float>();
            List<float> time = new List<float>();
            List<SubTask> subs = new List<SubTask>();
            string engName = subtasks[0].Engineer.UserName;
            string month;
            int i = -1;


            foreach (var item in subtasks)
            {
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
                if (item.ActualEndDate.Value.Month == DateTime.Now.Month)
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


        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersChart()
        {
            int teamId = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            List<ApplicationUser> teamMenmbers = unitOfWork.Engineers.GetEngineersInsideTeamWithSubTasks(teamId);
            teamMenmbers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);
            List<string> Ids = new List<string>();


            List<string> names = new List<string>();
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


        //public JsonResult TryJson()
        //{
        //    string engId = userManager.GetUserId(HttpContext.User);
        //    List<SubTask> subtasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engId);

        //    List<string> months = new List<string>();
        //    List<float> quality = new List<float>();
        //    List<float> complexity = new List<float>();
        //    List<float> time = new List<float>();
        //    List<int> subNo = new List<int>();
        //    List<float> totalDuration = new List<float>();
        //    List<float> tasksDuration = new List<float>();

        //    string engName = subtasks[0].Engineer.UserName;
        //    string month;
        //    int i = -1;


        //    foreach (var item in subtasks)
        //    {
        //        month = item.EndDate.Value.ToString("MMMM");
        //        if (!months.Contains(month))
        //        {
        //            months.Add(month);
        //            complexity.Add(item.Complexity);
        //            quality.Add(item.Quality * item.Complexity);
        //            time.Add(item.TimeManagement * item.Complexity);
        //            totalDuration.Add(item.ActualDuration);
        //            subNo.Add(1);
        //            i++;
        //        }
        //        else
        //        {
        //            complexity[i] += item.Complexity;
        //            quality[i] += item.Quality * item.Complexity;
        //            time[i] += item.TimeManagement * item.Complexity;
        //            totalDuration[i] += item.ActualDuration;
        //            subNo[i] += 1;
        //        }
        //    }

        //    for (int j = 0; j < months.Count(); j++)
        //    {
        //        quality[j] = quality[j] / complexity[j];
        //        time[j] = time[j] / complexity[j];
        //    }

        //    EngineerChrtViewModel engineerChrtView = new EngineerChrtViewModel
        //    {

        //        EngineerName = engName,
        //        Months = months,
        //        Quality = quality,
        //        Time = time,
        //        Complexity = complexity,

        //    };
        //    return Json(engineerChrtView);
        //}

        [HttpGet]
        public IActionResult displayCancellesSubTasks(string engId)
        {
            List<SubTask> subtasks = unitOfWork.SubTasks.GetEngineerSubTasks(engId);

            return PartialView(subtasks);
        }

        [HttpGet]
        public IActionResult displayAll()
        {
            Team team = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User));
            List<ApplicationUser> Engineers = unitOfWork.Engineers.GetEngineersWithSubtasks(team.Id).ToList();
            Engineers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);
            ResourceChartVM resourceChartVM = new ResourceChartVM
            {
                Employees = Engineers,
            };

            return View(resourceChartVM);
        }

        [HttpGet]
        public IActionResult displayCharts(string engId)
        {
            var subtask = unitOfWork.SubTasks.GetSubTasksByEngineerId(engId).ToList();
            return PartialView("_ChartsPartialView", subtask);
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


        public JsonResult GetApproximateDuration(string engineerId, int complexity, int Quality)
        {
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engineerId);
            SubTask newSubTask = new SubTask
            {
                Complexity = complexity,
                Quality = Quality
            };

            PredictedDuration predictedDuration = MLClass.PredictDurationBasedonQualityandCompl(subTasks, newSubTask);
            return Json(predictedDuration);
        }


        public JsonResult GetApproximateQuaity(string engineerId, int complexity, int Duration)
        {
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetEngineerComletedSubTasks(engineerId);
            SubTask newSubTask = new SubTask
            {
                Complexity = complexity,
                ActualDuration= Duration
            };

            PredictedQuality predictedQuality = MLClass.PredictQualityBasedonDurationandCompl(subTasks, newSubTask);
            return Json(predictedQuality);

        }



    }
}