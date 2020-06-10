using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
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



    }
}