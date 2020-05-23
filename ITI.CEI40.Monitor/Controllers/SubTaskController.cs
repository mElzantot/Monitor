using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
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
            return View("Engineer",subTasks);
        }

        public IActionResult DisplayRow(int ID)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            return PartialView("_SubTaskDataPartial", subTask);
        }

        public void EditProgress(int ID,int progress)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            subTask.Progress = progress;
            unitOfWork.SubTasks.Edit(subTask);
           // return Json(progress);
        }

        public void EditIsUnderWork(int ID, bool Is)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            subTask.IsUnderWork=Is;
            unitOfWork.SubTasks.Edit(subTask);
        }

        public void EditStatus(int ID,Status status)
        {
            SubTask subTask = unitOfWork.SubTasks.GetById(ID);
            subTask.Status = status;
            unitOfWork.SubTasks.Edit(subTask);
        }

    }
}