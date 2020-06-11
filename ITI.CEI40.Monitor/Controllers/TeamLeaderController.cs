using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TeamLeaderController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public TeamLeaderController(IUnitOfWork unitofwork,UserManager<ApplicationUser> userManager)
        {
            this.unitofwork = unitofwork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersView( )
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
        public void SubmitSubTask(int id, float complexity, float evaluation)
        {
            var subtask = unitofwork.SubTasks.GetSubTaskWithEngineer(id);
            subtask.Status = Status.Completed;
            subtask.Complexity = complexity;
            subtask.Evaluation = evaluation;
            //subtask.Engineer.TotalEvaluation;
            unitofwork.SubTasks.Edit(subtask);  
        }

        [HttpGet]
        public IActionResult displayCancellesSubTasks(string engId)
        {
            var subtask = unitofwork.SubTasks.GetEngineerSubTasks(engId);
            return PartialView(subtask);
        }
    }
}