using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TeamLeaderController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public TeamLeaderController(IUnitOfWork unitofwork, UserManager<ApplicationUser> userManager)
        {
            this.unitofwork = unitofwork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersView()
        {
            int teamId = unitofwork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            var engieers = unitofwork.Engineers.GetEngineersInsideTeam(teamId);
            return View(engieers);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult displaySubTasks(string engId)
        {
            List<SubTask> subtask = unitofwork.SubTasks.GetEngineerSubTasks(engId);
            return PartialView("_SubTaskPartialView", subtask);
        }


        //Omar 
        public void CancelSubTask(int id)
        {
            var subtask = unitofwork.SubTasks.GetById(id);
            subtask.Status = Status.Cancelled;
            unitofwork.SubTasks.Edit(subtask);
        }

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
            var subtasks = unitofwork.SubTasks.Archive(engId).ToList();
            return View("ArchivedSubTasks", subtasks);
        }

        //omar
        [Authorize(Roles = "Engineer")]
        public IActionResult EngineerChart()
        {
            
            string engId = userManager.GetUserId(HttpContext.User);
            List<SubTask> subtasks = unitofwork.SubTasks.GetEngineerComletedSubTasks(engId);


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
            int teamId = unitofwork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            List<ApplicationUser> team = unitofwork.Engineers.GetEngineersInsideTeamWithSubTasks(teamId);
            List<string> Ids = new List<string>();

            
            List<string> names = new List<string>();
            List<List<float>> avg = new List<List<float>>();

            foreach (var item in team)
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
                Names= names,
                Values= avg,
                EngIds=Ids
            };
            return View("TeamChart", teamChart);
        }



        private List<float> EngineerPerformence(List<SubTask> subTasks)
        {
            List<float> result = new List<float>() { 0,0,0,0,0};
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


        public JsonResult TryJson()
        {
            string engId = userManager.GetUserId(HttpContext.User);
            List<SubTask> subtasks = unitofwork.SubTasks.GetEngineerComletedSubTasks(engId);

            List<string> months = new List<string>();
            List<float> quality = new List<float>();
            List<float> complexity = new List<float>();
            List<float> time = new List<float>();
            List<int> subNo = new List<int>();
            List<float> totalDuration = new List<float>();
            List<float> tasksDuration = new List<float>();

            string engName = subtasks[0].Engineer.UserName;
            string month;
            int i = -1;


            foreach (var item in subtasks)
            {
                month = item.EndDate.Value.ToString("MMMM");
                if (!months.Contains(month))
                {
                    months.Add(month);
                    complexity.Add(item.Complexity);
                    quality.Add(item.Quality * item.Complexity);
                    time.Add(item.TimeManagement * item.Complexity);
                    totalDuration.Add(item.ActualDuration);
                    subNo.Add(1);
                    i++;
                }
                else
                {
                    complexity[i] += item.Complexity;
                    quality[i] += item.Quality * item.Complexity;
                    time[i] += item.TimeManagement * item.Complexity;
                    totalDuration[i] += item.ActualDuration;
                    subNo[i] += 1;
                }
            }

            for (int j = 0; j < months.Count(); j++)
            {
                quality[j] = quality[j] / complexity[j];
                time[j] = time[j] / complexity[j];
            }

            EngineerChrtViewModel engineerChrtView = new EngineerChrtViewModel
            {

                EngineerName = engName,
                Months = months,
                Quality = quality,
                Time = time,
                Complexity = complexity,

            };
            return Json(engineerChrtView);
        }

        [HttpGet]
        public IActionResult displayCancellesSubTasks(string engId)
        {
            List<SubTask> subtasks = unitofwork.SubTasks.GetEngineerSubTasks(engId);

            return PartialView(subtasks);
        }

        [HttpGet]
        public IActionResult displayAll(int teamid)
        {
            List<ApplicationUser> engieers = unitofwork.Engineers.GetEngineersWithSubtasks(teamid).ToList();
            return View(engieers);
        }

        [HttpGet]
        public IActionResult displayCharts(string engId)
        {
            var subtask = unitofwork.SubTasks.GetSubTasksByEngineerId(engId).ToList();
            return PartialView("_ChartsPartialView", subtask);
        }

    }
}