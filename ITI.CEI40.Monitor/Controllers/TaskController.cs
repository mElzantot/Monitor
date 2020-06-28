using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IHubContext<NotificationsHub> hubContext;


        public TaskController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.hubContext = hubContext;

        }


        //Views the tasks assigned to the team leader's team

        [Authorize(Roles = "TeamLeader")]
        public IActionResult ViewTasks()
        {
            //gets the team id using the logged in team leader id
            int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            //gets the tasks assigned to this team
            var tasks = unitOfWork.Tasks.GetTasksByTeamID(teamID);

            ViewBag.TeamId = teamID;
            return View(tasks);
        }

        //Allows the team leader to change the status of a task

        [Authorize(Roles = "TeamLeader")]
        public void EditStatus(int id, int status, string reasen)
        {
            //gets the task from database by its id
            Activity task = unitOfWork.Tasks.GetById(id);

            //changes the status of this task
            task.Status = (Status)status;

            //apply changes to data base
            unitOfWork.Tasks.Edit(task);

            //sends a notification to the department manager with the change of the task's status
            #region notification
            int teamID = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User)).Id;
            Team team = unitOfWork.Teams.GetById(teamID);
            ApplicationUser teammanager = userManager.Users.FirstOrDefault(u => u.Id == team.FK_TeamLeaderId);
            Department dep = unitOfWork.Departments.GetById(team.FK_DepartmentId);
            string messege;
            if (reasen == "" || reasen == null)
            {
                messege = $"*{teammanager.UserName}=* put the task *{task.Name}=* on Active status at *{DateTime.Now}=*.";
            }
            else
            {
                messege = $"*{teammanager.UserName}=* put the task *{task.Name}=* on Hold status because *{reasen}=* at *{DateTime.Now}=*.";
            }
            SendNotification(messege, dep.FK_ManagerID); 
            #endregion
        }


        //Shows the dashboard of each task

        [HttpGet]
        public IActionResult Dashboard(int taskId)
        {
            //gets the list of sub tasks inside a task by its id
            var subtask = unitOfWork.SubTasks.GetSubTasksFromTask(taskId);

            return View("_DashboardPartial", subtask);
        }



        //Uploads a file to the files hub

        [HttpPost]
        public IActionResult AddFile(FileViewModel addedFile)
        {
            string userId = userManager.GetUserId(HttpContext.User);
            //Activity task = unitOfWork.Tasks.GetById(addedFile.taskId);
            Comment fileComment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                FK_TaskID = addedFile.taskId,
                commentLevel = CommentLevels.High,
                comment = HttpContext.User.Identity.Name.ToString() + " uploaded File "
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



        //Adds a comment to the comments hub

        [HttpPost]
        public JsonResult AddComment(string comment, int taskId, int? subTaskId = null)
        {
            if (ModelState.IsValid)
            {
                string userId = userManager.GetUserId(HttpContext.User);
                //SubTask subTask = unitOfWork.SubTasks.GetById(subTaskId);

                Comment Comment = new Comment
                {
                    commentTime = DateTime.Now,
                    FK_sender = userId,
                    FK_TaskID = taskId,
                    comment = comment
                };
                return Json(new { result = true, msg = "Comment Added Successfully" });
            }

            return Json(new { result = false, msg = "Model Is not Valid" });

        }



        //adds description to a certain task

        [HttpPost]
        public IActionResult AddTaskDesc(int taskId, string taskDesc)
        {

            //gets the task from database by its id
            Activity task = unitOfWork.Tasks.GetById(taskId);

            if (taskDesc != null)
            {
                task.Description = taskDesc;
                task = unitOfWork.Tasks.Edit(task);
            }
            return RedirectToAction(nameof(GetTask), new { taskId = taskId });
        }




        //gets each task with its comments from the comments hub

        [HttpGet]
        public PartialViewResult GetTask(int taskId)
        {
            ActDetailsViewModel ActDetailsVM = new ActDetailsViewModel
            {
                //gets the task from database by its id
                Task = unitOfWork.Tasks.GetTaskWithProjectAndTeam(taskId),

                //gets this task comments 
                HighComments = unitOfWork.Comments.GetHighCommentforTask(taskId).ToList(),
            };
            return PartialView("_TaskDetailsPartialView", ActDetailsVM);
        }




        //function to send notifications to a certain user

        public void SendNotification(string messege, params string[] usersId)
        {
            Notification Notification = new Notification
            {
                messege = messege,
                seen = false
            };
            Notification Savednotification = unitOfWork.Notification.Add(Notification);

            for (int i = 0; i < usersId.Length; i++)
            {
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = usersId[i]
                };
                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Employee
                hubContext.Clients.User(usersId[i]).SendAsync("newNotification", messege, false, Savednotification.Id);
            }

        }


    }
}