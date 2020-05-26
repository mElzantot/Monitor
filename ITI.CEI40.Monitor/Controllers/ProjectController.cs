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
                Projects = unitofwork.Projects.GetRunningProjects(),
            };
            
            return View("_CreateProject", projectView);
        }


        [HttpPost]
        public JsonResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                project = unitofwork.Projects.Add(project);
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
            Project project = unitofwork.Projects.GetById(id);
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
    }
}