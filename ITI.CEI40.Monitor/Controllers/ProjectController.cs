using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
            ProjectViewModel projectViewModel = new ProjectViewModel
            {
                Projects = unitofwork.Projects.GetAll(),
            };
            return View("_CreateProject", projectViewModel);
        }


        [HttpPost]
        public JsonResult Add(ITI.CEI40.Monitor.Entities.Project project)
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


       public IActionResult Edit(int id)
       {
            ITI.CEI40.Monitor.Entities.Project project = unitofwork.Projects.GetById(id);
            if (project != null)
            {
                return PartialView("_FormPartial",project);
            }
            else
            {
                return null;
            }
       }

        [HttpPost]
        public IActionResult Edit(ITI.CEI40.Monitor.Entities.Project project)
        {
            if (ModelState.IsValid)
            {
                return PartialView("_FormPartial", project);
            }
            else
            {
                return null;
            }
        }
    }
}