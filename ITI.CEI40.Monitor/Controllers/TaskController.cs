using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult ViewTasks(int teamID)
        {
            var tasks = unitOfWork.Tasks.GetTasksByTeamID(teamID);
            ViewBag.TeamId = teamID;
            return View(tasks);
        }



        //[HttpGet]
        //public IActionResult Details(int projId)
        //{
        //    var project = unitOfWork.Projects.GetById(projId);
        //    return View(project);
        //}

        

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            var subtask = unitOfWork.SubTasks.GetSubTasksFromTask(taskId);
            return View("Dashboard", subtask);
        }
    }
}