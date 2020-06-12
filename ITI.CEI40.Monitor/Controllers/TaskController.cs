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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment hostingEnvironment;

        public TaskController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ViewTasks()
        {
            int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            var tasks = unitOfWork.Tasks.GetTasksByTeamID(teamID);
            ViewBag.TeamId = teamID;
            return View(tasks);
        }

        public void EditStatus(int id, int status)
        {
            Activity task = unitOfWork.Tasks.GetById(id);
            task.Status = (Status)status;
            unitOfWork.Tasks.Edit(task);
        }

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            var subtask = unitOfWork.SubTasks.GetSubTasksFromTask(taskId);
            return View("_DashboardPartial", subtask);
        }


        //[HttpGet]
        //public JsonResult AddFile (int subtaskId)
        //{


        //}





        [HttpPost]
        public IActionResult AddFile(FileViewModel addedFile)
        {
            string userId = userManager.GetUserId(HttpContext.User);
            SubTask subTask = unitOfWork.SubTasks.GetById(addedFile.taskId);
            Comment fileComment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                fk_TaskId = subTask.FK_TaskId,
                comment = addedFile.comment
            };

            fileComment = unitOfWork.Comments.Add(fileComment);

            //---------------- Add File---------------------//
            string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

            //---------Create New Guid for each File
            string uniqeFileName = Guid.NewGuid().ToString() + "_" + addedFile.file.FileName;

            //---------The full path for File
            string filePath = Path.Combine(uploaderFolder, addedFile.file.FileName);

            //----------Copy File to server
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                addedFile.file.CopyTo(fileStream);
            }

            //-----------Add File To DB
            Files newFile = new Files
            {
                FilePath = filePath,
                CommentID = fileComment.Id
            };

            newFile = unitOfWork.Files.Add(newFile);

            return RedirectToAction("Index", "SubTask");
        }


        [HttpPost]
        public JsonResult AddComment(string comment, int subTaskId)
        {
            if (ModelState.IsValid)
            {
            string userId = userManager.GetUserId(HttpContext.User);
            SubTask subTask = unitOfWork.SubTasks.GetById(subTaskId);

            Comment Comment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                fk_TaskId = subTask.FK_TaskId,
                comment = comment
            };
            return Json(new { result = true, msg = "Comment Added Successfully" });
            }

            return Json(new { result = false, msg = "Model Is not Valid" });

        }


    }
}