using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TeamLeaderController : Controller
    {

        private readonly IUnitOfWork unitofwork;
        public TeamLeaderController(IUnitOfWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "TeamLeader")]
        public IActionResult EngineersView(int teamid)
        {
            var engieers = unitofwork.Engineers.GetEngineersInsideTeam(teamid);
            return View(engieers);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult displaySubTasks(string engId)
        {
            List<SubTask> subtask = unitofwork.SubTasks.GetEngineerSubTasks(engId);
            return PartialView("_SubTaskPartialView", subtask);
        }


        //Omar to edit status if the team leader submit the subtask
        public void EditStatus(int id,int status)
        {
            var subtask = unitofwork.SubTasks.GetById(id);
            subtask.Status = (Status)status;
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