using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork unitofwork;
        private readonly IHostingEnvironment hostingEnvironment;
        public ProjectController(IUnitOfWork unitOfWork,IHostingEnvironment hostingEnvironment)
        {
            this.unitofwork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
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
            //Project project = unitofwork.Projects.GetById(id);

            ProjectViewModel projectVM = new ProjectViewModel
            {
                Project= unitofwork.Projects.GetById(id),
            };
            if (projectVM != null)
            {
                return PartialView("_FormPartial", projectVM);
            }
            else
            {
                return null;
            }
        }

        

        

        [HttpPost]
        public JsonResult Edit(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {

                Project project = new Project();
                project = model.Project;
                int projid = project.ID;
                string uniqeFileName = null;
                if (model.Files != null)
                {
                    //-------Get Files Folder path in Server
                    string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

                    foreach (var item in model.Files)
                    {
                        //-------Create New Guid fo each file
                        uniqeFileName =  item.FileName;
                        //---------The full path for file
                        string filePath = Path.Combine(uploaderFolder, uniqeFileName);
                        //----------Copy file to server
                        item.CopyTo(new FileStream(filePath, FileMode.Create));

                        //------Creat instance of file
                        Files file = new Files
                        {
                            FilePath = uniqeFileName,
                            FK_ProjectId = projid,
                            FK_SenderId=project.FK_Manager,
                            FileType = Entities.Enums.FileType.Project,
                            Time = DateTime.Now,


                        };

                        //-----Add file To DB
                        unitofwork.Files.Add(file);

                    }

                }
                unitofwork.Projects.Edit(model.Project);


                return Json(model.Project);
            }
            else
            {
                return Json(null);
            }
        }

        [HttpGet]
        public IActionResult CompletedProjects()
        {
            //--------Start Date will be the start date of first task of project
            var compProjects = unitofwork.Projects.GetCompletedProjects().ToList();
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