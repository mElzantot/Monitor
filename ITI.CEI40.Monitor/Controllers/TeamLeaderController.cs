using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
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

        public IActionResult EngineersView(int teamid)
        {
            var engieers = unitofwork.Engineers.GetEngineers(teamid);
            return View(engieers);
        }

        [HttpGet]
        public IActionResult displaySubTasks(string engId)
        {
            var subtask = unitofwork.EngineerSubTasks.GetSubTasksFromEngineerId(engId).ToList();            
            return PartialView("_SubTaskPartialView", subtask);
        }
        [HttpGet]
        public IActionResult displayCharts(string engId)
        {
            var subtask = unitofwork.EngineerSubTasks.GetSubTasksFromEngineerId(engId).ToList();
            return PartialView("_ChartsPartialView", subtask);
        }
    }
}