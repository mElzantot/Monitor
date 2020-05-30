using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class DepManagerController : Controller
    {
        private readonly IUnitOfWork unitofwork;
        public DepManagerController(IUnitOfWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeamsView(int DepId)
        {
            IEnumerable<Team> teams= unitofwork.Teams.getTeamsinsideDept(DepId);
            return View(teams);
        }

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            var task = unitofwork.Tasks.GetAllTaskWithTheirProject(teamId);
            return PartialView("_TaskPartialView", task);
        }

       
    }
}