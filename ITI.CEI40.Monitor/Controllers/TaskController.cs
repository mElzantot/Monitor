using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        public TaskController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult ViewTasks()
        {
            int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            var tasks = unitOfWork.Tasks.GetTasksByTeamID(teamID);
            ViewBag.TeamId = teamID;
            return View(tasks);
        }

        public void EditStatus(int id,int status)
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



        [HttpPost]
        public IActionResult AddFile(FileViewModel addedFile)
        {
            string userId = userManager.GetUserId(HttpContext.User);
            Comment fComment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                fk_TaskId = addedFile.taskId,
                comment = addedFile.comment
            };

            fComment = unitOfWork.Comments.Add(fComment);

            if (addedFile.file != null)
            {
                string filePath = "./wwwroot/file";
                filePath = Path.Combine(filePath, addedFile.file.FileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    addedFile.file.CopyTo(fileStream);
                }

                Files cFile = new Files
                {
                    FilePath = filePath,
                    CommentID = fComment.Id
                };
            }
            return RedirectToAction();
        }
    }
}