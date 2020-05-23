using System;
using System.Collections.Generic;
using System.Globalization;
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
                var endDate =   subTask.EndDate.Split('/').Select(Int32.Parse).ToList();
                var newSubTask = new SubTask
                    {
                        Name = subTask.Name,
                        Description = subTask.Description,
                        FK_TaskId = subTask.FK_TaskId,
                        FK_EngineerID = subTask.Assignee,
                        Priority = subTask.Priority,
                        Status = subTask.Status,
                        StartDate = new DateTime(startDate[2],startDate[1],startDate[0]),
                        EndDate = new DateTime(endDate[2],endDate[1],endDate[0])
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