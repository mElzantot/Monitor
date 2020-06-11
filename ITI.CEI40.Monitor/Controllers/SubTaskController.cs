using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ITI.CEI40.Monitor.Controllers
{
    public class SubTaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;
    

        public SubTaskController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hubContext = hubContext;
           
        }



        [Authorize(Roles = "Engineer")]
        public IActionResult Index()
        {
            //----------- Get user Id from UserManager ---------//
            string engId = userManager.GetUserId(HttpContext.User);
            List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByEngineerId(engId).ToList();
            return View("Engineer", subTasks);
        }

        [Authorize(Roles = "Engineer")]
        public IActionResult DisplayRow(int ID)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingTask(ID);
            return PartialView("_SubTaskDataPartial", subTask);
        }

        [Authorize(Roles = "Engineer")]
        public void EditProgress(int ID, int progress)        {            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingTask(ID);

            int subTaskLastProgress = subTask.Progress;

            //Shaker
            //Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
            Activity task = subTask.Task;

            List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(subTask.FK_TaskId);            int totalSubTaskDuration = 0;            foreach (var item in subTasks)            {                totalSubTaskDuration += (int)(item.EndDate - item.StartDate).Value.TotalDays;            }            int subtaskDuration = (int)(subTask.EndDate - subTask.StartDate).Value.TotalDays;            task.Progress += ((progress - subTaskLastProgress) * (subtaskDuration)) / (totalSubTaskDuration);            task = unitOfWork.Tasks.Edit(task);
            subTask.Progress = progress;            subTask = unitOfWork.SubTasks.Edit(subTask);        }

        [Authorize(Roles = "Engineer")]
        public void EditIsUnderWork(int ID, bool Is)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingProject(ID);
            subTask.IsUnderWork = Is;

            if (Is)
            {
                SubTaskSession subTaskSession = new SubTaskSession()
                {
                    FK_SubTaskID = ID,
                    SessStartDate = DateTime.Now
                };
                subTaskSession = unitOfWork.SubTaskSessions.Add(subTaskSession);
            }
            else
            {
                SubTaskSession subTaskSession = unitOfWork.SubTaskSessions.GetLastSessBySubTaskID(ID);
                subTaskSession.SessEndtDate = DateTime.Now;
                double hourDuration = (double)(subTaskSession.SessEndtDate - subTaskSession.SessStartDate).Value.TotalHours;
                subTaskSession.SessDuration = (int)Math.Round(hourDuration, 0);
                subTaskSession = unitOfWork.SubTaskSessions.Edit(subTaskSession);

                subTask.ActualDuration += subTaskSession.SessDuration;

                Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
                int LastTaskDuaration = task.ActualDuratoin;
                task.ActualDuratoin += subTask.ActualDuration;
                task = unitOfWork.Tasks.Edit(task);
                Project project = unitOfWork.Projects.GetById(task.FK_ProjectId);
                project.ActualDuration += task.ActualDuratoin - LastTaskDuaration;
                unitOfWork.Projects.Edit(project);

            }
            subTask = unitOfWork.SubTasks.Edit(subTask);
        }

        [Authorize(Roles = "Engineer")]
        public void EditStatus(int id, int status, string reason)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskWithTeam(id);
            subTask.Status = (Status)status;
            unitOfWork.SubTasks.Edit(subTask);

            Comment comment = new Comment
            {
                FK_sender = userManager.GetUserId(HttpContext.User),
                fk_TaskId = subTask.FK_TaskId,
                commentTime = DateTime.Now
            };
            comment.comment = $"{subTask.Name} status has changed to {subTask.Status.ToString()}";
            comment.comment += reason;
            comment = unitOfWork.Comments.Add(comment);

            //hubContext.Clients.User(subTask.Task.Team.FK_TeamLeaderId).SendAsync("newNotification",
            // $"");
        }


        [Authorize(Roles = "Engineer")]
        public void EditPriority(int ID, Priority priority)
        {
            Activity task = unitOfWork.Tasks.GetById(ID);
            task.Priority = priority;
            unitOfWork.Tasks.Edit(task);
        }


        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult displaySubTasks(int taskID)
        {
            var taskVM = new TaskViewModel
            {
                TaskId = taskID,
                taskDescription = unitOfWork.Tasks.GetById(taskID).Description,
                SubTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(taskID),
            };
            return PartialView("_SubTaskDisplayPartial", taskVM);

        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult AddSubTask(int taskID, int teamId)
        {
            var subTask = new SubTaskViewModel
            {
                FK_TaskId = taskID,
                TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList()
            };
            return PartialView("_SubTaskModal", subTask);
        }

        [Authorize(Roles = "Team Leader")]
        [HttpPost]
        public IActionResult AddSubTask(SubTaskViewModel subTask)
        {
            if (ModelState.IsValid)
            {
                var startDate = subTask.StartDate.Split('/').Select(Int32.Parse).ToList();
                var endDate = subTask.EndDate.Split('/').Select(Int32.Parse).ToList();
                var newSubTask = new SubTask
                {
                    Name = subTask.Name,
                    Description = subTask.Description,
                    FK_TaskId = subTask.FK_TaskId,
                    FK_EngineerID = subTask.Assignee,
                    Priority = subTask.Priority,
                    Status = subTask.Status,
                    StartDate = new DateTime(startDate[2], startDate[1], startDate[0]),
                    EndDate = new DateTime(endDate[2], endDate[1], endDate[0])
                };

                newSubTask = unitOfWork.SubTasks.Add(newSubTask);
                newSubTask.Engineer = unitOfWork.Engineers.GetById(subTask.Assignee);


                //--------Add Notification to DataBase

                Notification Notification = new Notification
                {
                    Action = Entities.Enums.NotificationType.Assigned,
                    TaskName = newSubTask.Name,
                    FK_SenderId = userManager.GetUserId(HttpContext.User),
                    NotifTime = DateTime.Now
                };

                Notification Savednotification = unitOfWork.Notification.Add(Notification);
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = newSubTask.FK_EngineerID
                };

                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Employee

                string message = $"New Task ," +
                    $" There Is a new task -{Notification.TaskName}- assigned to you at {Notification.NotifTime.Date} ";

                hubContext.Clients.User(newSubTask.FK_EngineerID).SendAsync("newNotification", message);


                return PartialView("_NewSubTaskPartialView", newSubTask);

            }
            else
            {
                return null;
            }
        }

    }
}