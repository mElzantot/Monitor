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
                SubTaskSession opensubtaskSession = unitOfWork.SubTaskSessions.GetOpenSubTask(userManager.GetUserId(HttpContext.User));
                if (opensubtaskSession != null)
                {
                    CloseOpenSubTasksession(opensubtaskSession, opensubtaskSession.SubTask);
                    unitOfWork.SubTasks.Edit(opensubtaskSession.SubTask);
                }

                SubTaskSession subTaskSession = new SubTaskSession()
                {
                    FK_SubTaskID = ID,
                    SessStartDate = DateTime.Now,
                    EmpId = userManager.GetUserId(HttpContext.User)
                };
                subTaskSession = unitOfWork.SubTaskSessions.Add(subTaskSession);
            }
            else
            {
                SubTaskSession subTaskSession = unitOfWork.SubTaskSessions.GetLastSessBySubTaskID(ID);
                CloseOpenSubTasksession(subTaskSession, subTask);

            }
            subTask = unitOfWork.SubTasks.Edit(subTask);
        }

        [Authorize(Roles = "Engineer")]
        public void EditStatus(int id, int status, string reason)
        {
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskWithTeam(id);
            subTask.Status = (Status)status;
            subTask = unitOfWork.SubTasks.Edit(subTask);

            #region notification
            //--------Add Notification to DataBase

            string messege = $"*{subTask.Name}=*'s status has been updated to *{subTask.Status.ToString()}=* " +
                $"at *{DateTime.Now}=* ";
            Notification Notification = new Notification
            {
                messege = messege,
                seen = false
            };
            Notification Savednotification = unitOfWork.Notification.Add(Notification);
            NotificationUsers notificationUsers = new NotificationUsers
            {
                NotificationId = Savednotification.Id,
                userID = subTask.Task.Team.FK_TeamLeaderId
            };
            unitOfWork.NotificationUsers.Add(notificationUsers);

            //---------Send Notification to Employee
            hubContext.Clients.User(subTask.Task.Team.FK_TeamLeaderId).SendAsync("newNotification", messege, false, Savednotification.Id);
            #endregion


            Comment comment = new Comment
            {
                FK_sender = userManager.GetUserId(HttpContext.User),
                FK_TaskID = subTask.FK_TaskId,
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
            Activity task = unitOfWork.Tasks.GetById(taskID);
            var taskVM = new TaskViewModel
            {
                TaskId = taskID,
                TaskName = task.Name,
                TaskEndDate = task.EndDate,
                taskDescription = task.Description,
                SubTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(taskID),
            };
            return PartialView("_SubTaskDisplayPartial", taskVM);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult AddSubTask(int taskID, int teamId)
        {
            List<ApplicationUser> TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList();
            TeamMembers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);

            var subTask = new SubTaskViewModel
            {
                FK_TaskId = taskID,
                TeamMembers = TeamMembers
            };
            return PartialView("_SubTaskModal", subTask);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpPost]
        public IActionResult AddSubTask(SubTaskViewModel subTask)
        {
            if (ModelState.IsValid)
            {
                var startDate = subTask.StartDate.Split('/').Select(Int32.Parse).ToList();
                var endDate = subTask.EndDate.Split('/').Select(Int32.Parse).ToList();
                subTask.Description = subTask.Description.Replace("\r\n", "<br>");
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

                #region notification

                //--------Add Notification to DataBase

                string messege = $" There Is a new task *{newSubTask.Name}=* had been assigned to you" +
                    $" at *{DateTime.Now}=* ";

                SendNotification(messege, newSubTask.FK_EngineerID);
                #endregion

                return PartialView("_NewSubTaskPartialView", newSubTask);

            }
            else
            {
                return null;
            }
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpGet]
        public IActionResult EditSubTask(int subtaskId)
        {
            Team team = unitOfWork.Teams.GetTeamWithTeamLeaderId(userManager.GetUserId(HttpContext.User));
            SubTask subTask = unitOfWork.SubTasks.GetSubTaskWithEngineer(subtaskId);
            List<ApplicationUser> TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(team.Id).ToList();
            TeamMembers.RemoveAll(e => e.UserName == HttpContext.User.Identity.Name);

            var subTaskVM = new SubTaskViewModel
            {
                SubTaskId = subtaskId,
                Name = subTask.Name,
                Description = subTask.Description,
                StartDate = subTask.StartDate.Value.Date.ToShortDateString() ?? "",
                EndDate = subTask.EndDate.Value.Date.ToShortDateString() ?? "",
                Assignee = subTask.Engineer.Id,
                Status = subTask.Status,
                Priority = subTask.Priority,
                TeamMembers = TeamMembers
            };

            var index = subTaskVM.TeamMembers.FindIndex(x => x.Id == subTaskVM.Assignee.ToString());
            var item = subTaskVM.TeamMembers[index];
            subTaskVM.TeamMembers[index] = subTaskVM.TeamMembers[0];
            subTaskVM.TeamMembers[0] = item;

            //subTaskVM.TeamMembers.OrderBy(x => x.Id == subTask.FK_EngineerID).ToList();

            return PartialView("_SubTaskModal", subTaskVM);
        }

        [Authorize(Roles = "TeamLeader")]
        [HttpPost]
        public IActionResult EditSubTask(SubTaskViewModel newSubTask)
        {

            if (ModelState.IsValid)
            {
                int[] startDate = new int[3];
                int[] endDate = new int[3];
                bool ChangeAssign = false;
                SubTask originalSubTask = unitOfWork.SubTasks.GetSubTaskWithEngineer(newSubTask.SubTaskId);
                // check if the assigneed engineer had been changed
                string oldAssigneeId = originalSubTask.FK_EngineerID;
                if (originalSubTask.FK_EngineerID != newSubTask.Assignee)
                {
                    originalSubTask.FK_EngineerID = newSubTask.Assignee;
                    ChangeAssign = true;
                }
                // edit the subtask
                originalSubTask.Name = newSubTask.Name;
                originalSubTask.Description = newSubTask.Description;
                originalSubTask.Priority = newSubTask.Priority;
                originalSubTask.Status = newSubTask.Status;

                if (newSubTask.StartDate.Contains('/'))
                {
                    startDate = newSubTask.StartDate.Split('/').Select(Int32.Parse).ToArray();
                    originalSubTask.StartDate = new DateTime(startDate[2], startDate[1], startDate[0]);
                }
                if (newSubTask.EndDate.Contains('/'))
                {
                    endDate = newSubTask.EndDate.Split('/').Select(Int32.Parse).ToArray();
                    originalSubTask.EndDate = new DateTime(endDate[2], endDate[1], endDate[0]);
                }
                originalSubTask = unitOfWork.SubTasks.Edit(originalSubTask);

                // check if the subtask had been changed
                if (!ChangeAssign)
                {
                    #region notification without assignee changed
                    //--------Add Notification to DataBase

                    string messege = $"Your Team Leader has updated *{originalSubTask.Name}=*'s details  at *{DateTime.Now}=* ";

                    SendNotification(messege, originalSubTask.FK_EngineerID);

                    #endregion
                }
                else
                {
                    #region notification for old employee
                    //--------Add Notification to DataBase

                    string messege1 = $"Your Team Leader has reassigned *{originalSubTask.Name}=* to another employee  at *{DateTime.Now}=* ";
                    SendNotification(messege1, oldAssigneeId);
                    #endregion

                    #region notification for the new employee
                    //--------Add Notification to DataBase

                    string messege2 = $"Your Team Leader has updated *{originalSubTask.Name}=*'s details  at *{DateTime.Now}=* ";
                    SendNotification(messege2, newSubTask.Assignee);
                    #endregion

                }


                originalSubTask = unitOfWork.SubTasks.GetSubTaskWithEngineer(originalSubTask.Id);
                return PartialView("_NewSubTaskPartialView", originalSubTask);
            }
            else
            {
                return null;
            }
        }

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

        public void CloseOpenSubTasksession(SubTaskSession subTaskSession, SubTask subTask)        {            subTaskSession.SessEndtDate = DateTime.Now;            double hourDuration = (double)(subTaskSession.SessEndtDate - subTaskSession.SessStartDate).Value.TotalHours;            subTaskSession.SessDuration = (int)Math.Round(hourDuration, 0);            subTaskSession = unitOfWork.SubTaskSessions.Edit(subTaskSession);            subTask.ActualDuration += subTaskSession.SessDuration;            subTask.IsUnderWork = false;            Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);            float LastTaskDuaration = task.ActualDuratoin;            task.ActualDuratoin += subTask.ActualDuration;            task = unitOfWork.Tasks.Edit(task);            Project project = unitOfWork.Projects.GetById(task.FK_ProjectId);            project.ActualDuration += task.ActualDuratoin - LastTaskDuaration;            unitOfWork.Projects.Edit(project);        }


    }
}