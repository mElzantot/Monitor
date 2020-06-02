using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class SubTaskController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public SubTaskController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index(string engineerID)
        {
            IEnumerable<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByEngineerId(engineerID);
            return View("Engineer", subTasks);
        }

        public IActionResult DisplayRow(int ID)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            return PartialView("_SubTaskDataPartial", subTask);
        }


        public void EditProgress(int ID, int progress)        {            SubTask subTask = unitOfWork.SubTasks.GetSubTaskIncludingTask(ID);

            int subTaskLastProgress = subTask.Progress;

            //Shaker
            //Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
            Activity task = subTask.Task;

            List<SubTask> subTasks = unitOfWork.SubTasks.GetSubTasksByTaskId(subTask.FK_TaskId);            int totalSubTaskDuration = 0;            foreach (var item in subTasks)            {                totalSubTaskDuration += (int)(item.EndDate - item.StartDate).Value.TotalDays;            }            int subtaskDuration = (int)(subTask.EndDate - subTask.StartDate).Value.TotalDays;            task.Progress += ((progress - subTaskLastProgress) * (subtaskDuration)) / (totalSubTaskDuration);            task = unitOfWork.Tasks.Edit(task);
            subTask.Progress = progress;            subTask = unitOfWork.SubTasks.Edit(subTask);        }

        


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

                //Activity task = unitOfWork.Tasks.GetById(subTask.FK_TaskId);
                Activity task = subTask.Task;
                int LastTaskDuaration = task.ActualDuratin;
                task.ActualDuratin += subTask.ActualDuration;
                task = unitOfWork.Tasks.Edit(task);
                // Project project = unitOfWork.Projects.GetById(task.FK_ProjectId);
                Project project = subTask.Task.Project;
                project.ActualDuration += task.ActualDuratin - LastTaskDuaration;
                unitOfWork.Projects.Edit(project);

            }
            subTask = unitOfWork.SubTasks.Edit(subTask);
        }

        public void EditStatus(int ID, Status status)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            subTask.Status = status;
            unitOfWork.SubTasks.Edit(subTask);
        }
        public void EditPriority(int ID, Priority priority)
        {
            Activity task = unitOfWork.Tasks.GetById(ID);
            task.Priority = priority;
            unitOfWork.Tasks.Edit(task);
        }




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
                return PartialView("_NewSubTaskPartialView", newSubTask);

            }
            else
            {
                return null;
            }
        }



    }
}