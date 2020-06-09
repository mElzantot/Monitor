using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITI.CEI40.Monitor.Controllers
{
    public class DepManagerController : Controller
    {
        private readonly IUnitOfWork unitofwork;

        public DepManagerController(IUnitOfWork unitofwork)
        {
            this.unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TeamsView(int DepId)
        {
            IEnumerable<Team> teams = unitofwork.Teams.getTeamsinsideDept(DepId);
            return View(teams);
        }

        [HttpGet]
        public IActionResult displayTasks(int teamId)
        {
            var tasks = unitofwork.Tasks.GetHoldActiveTasks(teamId);
            return PartialView("_TaskPartialView", tasks);
        }

        [HttpGet]
        public IActionResult AssignTasks(int depid)
        {
            var activityVM = new ActivityViewModel();

            activityVM.Tasks = unitofwork.Tasks.GetDepartmentTasks(depid);

            activityVM.Teams = unitofwork.Teams.getTeamsinsideDept(depid).ToList();

            return View(activityVM);
        }



        [HttpPost]
        public JsonResult AssignTasks(int taskId, int teamId)
        {
            if (ModelState.IsValid)
            {
                var task = unitofwork.Tasks.GetById(taskId);
                task.FK_TeamId = teamId;
                var team = unitofwork.Teams.GetById(teamId);
                unitofwork.Complete();
                return Json(new { teamName = team.Name });
            }
            return Json(new { });
        }

       
        //Omar to edit status if the Departement manager submit the subtask
        public void EditStatus(int id, int status)
        {
            var task = unitofwork.Tasks.GetById(id);
            task.Status = (Status)status;
            if (task.Status == Status.Cancelled)
            {
                List<SubTask> subTasks = unitofwork.SubTasks.GetSubTasksByTaskId(task.Id);
                foreach (var item in subTasks)
                {
                    item.Status = Status.Cancelled;
                    //Want to Edit // Make Edit for List of items
                    unitofwork.SubTasks.Edit(item);
                }
            }
            unitofwork.Tasks.Edit(task);
        }

        [HttpGet]
        public IActionResult CancelledTasks(int depid)
        {
            var tasks = unitofwork.Tasks.GetDepCancelledTasks(depid).ToList();
            return View( tasks);
        }

        
    }
}