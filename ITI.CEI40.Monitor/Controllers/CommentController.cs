using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class CommentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> usermanager;

        public CommentController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> usermanager)
        {
            this.unitOfWork = unitOfWork;
            this.usermanager = usermanager;
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

            //List<Comment> comments = unitOfWork.Comments.GetLowCommentforTask(taskId).ToList();

            List<SubTask> subTasks = unitOfWork.SubTasks.GetAllSubTasksWithTask(taskId);

            //CommentViewModel commentView = new CommentViewModel
            //{
            //    Comments = comments,
            //    SubTasks = subTasks
            //};
            return View("ActivityLog", subTasks);
        }


        //-----------Link Between Engineer and Team Leader
        [HttpPost]
        public JsonResult AddCommentForSubTask(string comment, int subTaskId, int taskId)
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
                return Json(new { result = true, msg = "Comment Added Successfully" });
            }
            return Json(new { result = false, msg = "Model Is not Valid" });
        }


        //--------Comments That invisible for Engineers
        [HttpPost]
        public JsonResult AddCommentForTask(string comment, int taskId, bool isHighLevel)
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
                return Json(new { result = true, msg = "Comment Added Successfully" });
            }
            return Json(new { result = false, msg = "Model Is not Valid" });
        }







    }
}