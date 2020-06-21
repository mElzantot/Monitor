using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly IHostingEnvironment hostingEnvironment;

        public CommentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> usermanager, IHostingEnvironment hostingEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.usermanager = usermanager;
            this.hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            List<SubTask> Subtasks = unitOfWork.SubTasks.GetEngineerSubTasks(usermanager.GetUserId(HttpContext.User));
            List<int> TasksIds = null;
            List<TaskCommentsModelView> TasksMV = null;

            foreach (var item in Subtasks)
            {
                if (!TasksIds.Contains(item.FK_TaskId))
                {
                    TasksIds.Add(item.FK_TaskId);
                }
            }

            if (TasksIds != null)
            {
                Activity Task = null;
                foreach (var item in TasksIds)
                {
                    Task = unitOfWork.Tasks.GetById(item);
                    TasksMV.Add(new TaskCommentsModelView { TaskId = Task.Id, TaskName = Task.Name });
                }
            }
            return View(TasksMV);
        }

        public IActionResult ActivityLog(int taskId)
        {
            string userId = usermanager.GetUserId(HttpContext.User);
            //-------Get All Comments for Team Leader
            List<Comment> taskComments = unitOfWork.Comments.GetCommentsForTeamLeader(taskId).ToList();

            //-------Get SubTasks with Task
            List<SubTask> subtasks = unitOfWork.SubTasks.GetSubTasksByTaskId(taskId);
            //--------Group Comments
            var CommentsGroups = taskComments.GroupBy(c => c.FK_SubTaskId);
            //-----------Task Comments
            var comments = CommentsGroups.Where(c => c.Key == null).ToList();
            if (comments.Count > 0)
            {
                subtasks[0].Task.Comments = comments[0].ToList();
            }

            //---------SubTasks Comments
            foreach (SubTask subtask in subtasks)
            {
                comments = CommentsGroups.Where(c => c.Key == subtask.Id).ToList();
                if (comments.Count > 0)
                {
                    subtask.Comments = comments[0].ToList() as List<Comment>;
                }
            }
            return View("ActivityLog", subtasks);
        }


        [HttpPost]
        public IActionResult AddFileForSubTask(FileViewModel addedFile)
        {
            string userId = usermanager.GetUserId(HttpContext.User);

            Comment fileComment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                FK_SubTaskId = addedFile.subTaskId,
                FK_TaskID = addedFile.taskId,
                commentLevel = CommentLevels.low,
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
                FilePath = uniqeFileName,
                CommentID = fileComment.Id
            };

            newFile = unitOfWork.Files.Add(newFile);

            fileComment.File = newFile;
            return PartialView("_CommentPartial", fileComment);
        }

        //-----------Link Between Engineer and Team Leader
        [HttpPost]
        public IActionResult AddCommentForSubTask(string comment, int subTaskId, int taskId)
        {
            if (ModelState.IsValid)
            {
                string userId = usermanager.GetUserId(HttpContext.User);

                Comment Comment = new Comment
                {
                    commentTime = DateTime.Now,
                    FK_sender = userId,
                    FK_SubTaskId = subTaskId,
                    FK_TaskID = taskId,
                    commentLevel = CommentLevels.low,
                    comment = comment
                };

                Comment = unitOfWork.Comments.Add(Comment);
                return PartialView("_CommentPartial", Comment);
            }
            return null;
        }

        //--------Comments That invisible for Engineers
        [HttpPost]
        public IActionResult AddCommentForTask(string comment, int taskId, bool isHighLevel)
        {
            if (ModelState.IsValid)
            {
                string userId = usermanager.GetUserId(HttpContext.User);

                Comment Comment = new Comment
                {
                    commentTime = DateTime.Now,
                    FK_sender = userId,
                    FK_TaskID = taskId,
                    comment = comment
                };
                if (isHighLevel) { Comment.commentLevel = CommentLevels.High; }
                else { Comment.commentLevel = CommentLevels.Med; }
                Comment = unitOfWork.Comments.Add(Comment);
                return PartialView("_CommentPartial", Comment);

            }
            return null;
        }

        [HttpPost]
        public IActionResult AddFileForTask([FromForm]FileViewModel addedFile, [FromQuery]bool isHighLevel)
        {
            string userId = usermanager.GetUserId(HttpContext.User);

            Comment fileComment = new Comment
            {
                commentTime = DateTime.Now,
                FK_sender = userId,
                FK_TaskID = addedFile.taskId,
                comment = HttpContext.User.Identity.Name.ToString() + " uploaded File "
            };
            if (isHighLevel) { fileComment.commentLevel = CommentLevels.High; }
            else { fileComment.commentLevel = CommentLevels.Med; }
            fileComment = unitOfWork.Comments.Add(fileComment);

            //---------------- Add File---------------------//
            string uploaderFolder = Path.Combine(hostingEnvironment.WebRootPath, "files");

            //---------Create New Guid for each File
            string uniqeFileName = Guid.NewGuid().ToString() + "_" + addedFile.file.FileName;

            //---------The full path for File
            string filePath = Path.Combine(uploaderFolder,uniqeFileName);

            //----------Copy File to server
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                addedFile.file.CopyTo(fileStream);
            }

            //-----------Add File To DB
            Files newFile = new Files
            {
                FilePath = uniqeFileName,
                CommentID = fileComment.Id
            };
            newFile = unitOfWork.Files.Add(newFile);
            fileComment.File = newFile;

            return PartialView("_CommentPartial", fileComment);


        }

    }
}