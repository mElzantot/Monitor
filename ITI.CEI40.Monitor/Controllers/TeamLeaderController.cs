using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
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

        //omar
        public IActionResult EngineerChart(string EngId)
        {
            List<SubTask> subtasks = unitofwork.SubTasks.GetEngineerComletedSubTasks(EngId);

            List<string> months = new List<string>();
            List<float> quality = new List<float>();
            List<float> complexity = new List<float>();
            List<float> time = new List<float>();
            List<int> subNo = new List<int>();

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
                    subNo.Add(1);
                    i++;
                }
                else
                {
                    complexity[i] += item.Complexity;
                    quality[i] += item.Quality * item.Complexity;
                    time[i] += item.TimeManagement * item.Complexity;
                    subNo[i] += 1;
                }
            }

            for (int j = 0; j < months.Count(); j++)
            {
                quality[j] = quality[j] / complexity[j];
                time[j] = quality[j] / complexity[j];
            }

            EngineerChrtViewModel engineerChrtView = new EngineerChrtViewModel
            {
                SubMonth = subNo,
                EngineerName = engName,
                Months = months,
                Quality = quality,
                Time = time
            };

            return View("EngineerChart", engineerChrtView);
        }

        [HttpGet]
        public IActionResult displayCancellesSubTasks(string engId)
        {
            List<SubTask> subtasks = unitofwork.SubTasks.GetEngineerSubTasks(engId);

            return PartialView(subtasks);
        }


        [HttpGet]
        public void SmartDes()
        {
            string engId = "20744706-e13b-486b-bead-8c8b08b78650";
            string engId2 = "30fd099c-7885-4bc3-8399-17fe761623ee";
            List<SubTask> subTasks = unitofwork.SubTasks.GetEngineerComletedSubTasks(engId);
            List<SubTask> subTasks2 = unitofwork.SubTasks.GetEngineerComletedSubTasks(engId2);

            subTasks.AddRange(subTasks2);

            SubTask TargetsubTask = new SubTask
            {
                Complexity = 20,
                Quality = 95,
                FK_EngineerID = engId2
            };

            PredictedDuration predictedDuration = MLClass.PredictDurationBasedonQualityandCompl(subTasks2, TargetsubTask);

        }
    }
}