using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Models;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{

    [Authorize(Roles = "ProjectManager")]
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            this.unitofwork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            ProjectViewModel projectView = new ProjectViewModel
            {
                Projects = unitofwork.Projects.GetRunningProjects(userManager.GetUserId(HttpContext.User)),
            };

            return View("_CreateProject", projectView);
        }


        [HttpPost]
        public JsonResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                project.FK_Manager = userManager.GetUserId(HttpContext.User);
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

        [HttpGet]
        public IActionResult CompletedProjects()
        {
            //--------Start Date will be the start date of first task of project
            var compProjects = unitofwork.Projects.GetCompletedProjects(userManager.GetUserId(HttpContext.User)).ToList();
            List<CompletedProjectsViewModel> CompletedProjectsVM = new List<CompletedProjectsViewModel>();
            foreach (var item in compProjects)
            {
                CompletedProjectsViewModel compProject = new CompletedProjectsViewModel(item);
                CompletedProjectsVM.Add(compProject);
            }

            return View(CompletedProjectsVM);
        }

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

        public IActionResult ProjectDailyReport(int Id)
        {
            Project project = unitofwork.Projects.GetProjectForReport(Id);
            return View(project);
        }



    }
}