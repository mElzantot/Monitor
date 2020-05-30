using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models;
using Microsoft.AspNetCore.Mvc;


namespace ITI.CEI40.Monitor.Controllers
{
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork unitofwork;
        public ProjectController(IUnitOfWork unitOfWork)
        {
            this.unitofwork = unitOfWork;
        }


        public IActionResult Index()
        {
            ProjectViewModel projectView = new ProjectViewModel
            {
                Projects = unitofwork.Projects.GetAll(),
            };

            return View("_CreateProject", projectView);
        }


        [HttpPost]
        public JsonResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                unitofwork.Projects.Add(project);
                return Json(project);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ITI.CEI40.Monitor.Entities.Project project = unitofwork.Projects.GetById(id);
            if (project != null)
            {
                return PartialView("_FormPartial", project);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                unitofwork.Projects.Edit(project);
                return Json(project);
            }
            else
            {
                return Json(project);
            }
        }

        [HttpGet]
        public IActionResult DisplayProjects()
        {
            var project = unitofwork.Projects.GetAllProjects().ToList();
            return View(project);
        }

        [HttpGet]
        public IActionResult DashBoard(int projId)
        {
            //Project project = unitofwork.Projects.GetProjectWithTasks(projId);
            List<Activity> tasks = unitofwork.Tasks.GetActivitiesFromProject(projId).ToList();
            return PartialView("_DashBoardPartial", tasks);
        }


    }
}