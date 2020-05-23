using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
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
        public IActionResult AddSubTask(int  taskID,int teamId)
        {
            var subTask = new SubTaskViewModel
            {
                FK_TaskId = taskID,
                TeamMembers = unitOfWork.Engineers.GetEngineersInsideTeam(teamId).ToList()
            };
            return PartialView("_SubTaskModal",subTask);
        }
    }
}