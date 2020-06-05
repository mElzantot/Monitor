using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment hostingEnvironment;

        public TaskController(IUnitOfWork unitOfWork,IHostingEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ViewTasks(int teamID)
        {
            var tasks = unitOfWork.Tasks.GetTasksByTeamID(teamID);
            ViewBag.TeamId = teamID;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult AddFile(int taskid)
        {
            TaskViewModel taskVM = new TaskViewModel
            {
                Task = unitOfWork.Tasks.GetTaskWithProject(taskid),
            };

            return View( taskVM);
        }

        [HttpPost]
        public IActionResult AddFile(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {

                Activity task = new Activity();
                task = model.Task;

                int taskid = task.Id;

                string uniqeFileName = null;
                if (model.Files != null)
                {
                    //-------Get files Folder path in Server
                    string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

                    foreach (var item in model.Files)
                    {
                        //-------Create New Guid fo each file
                        uniqeFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        //---------The full path for file
                        string filePath = Path.Combine(uploaderFolder, uniqeFileName);
                        //----------Copy file to server
                        item.CopyTo(new FileStream(filePath, FileMode.Create));

                        //------Creat instance of file
                        Files file = new Files
                        {
                            FilePath = uniqeFileName,
                            FK_TaskId=taskid,
                            //FK_SenderId=model.Task.Team.FK_TeamLeaderId,
                            FileType = Entities.Enums.FileType.Subtask,
                            Time = DateTime.Now,


                        };

                        //-----Add image To DB
                        unitOfWork.Files.Add(file);

                    }
                }
                return View(model);
            }
            else
                return View("Error");
        }

    }
}