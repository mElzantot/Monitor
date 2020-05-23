﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
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
    }
}