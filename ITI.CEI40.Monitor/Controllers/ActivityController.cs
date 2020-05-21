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
    public class ActivityController : Controller
    {
        private IUnitOfWork unitOfWork;
        public ActivityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index(int id)
        {
            ActivityViwewModel activityVM = new ActivityViwewModel
            {
                ProjectId = id,
                Activities = unitOfWork.Tasks.GetByProjectId(id).ToList()
            };
            return View(activityVM);
        }

        [HttpPost]
        public IActionResult AddActivities(int id, List<Act> Acts)
        {
            //deleting old tasks
            IEnumerable<Activity> OldActs = unitOfWork.Tasks.GetByProjectId(id).ToList();
            if (OldActs != null || OldActs.Count() != 0)
            {
                foreach (var item in OldActs)
                {
                    unitOfWork.Tasks.Delete(item.Id);
                }
            }
            DateTime _jan1st1970 = new DateTime(1970, 1, 1, 23, 59, 59, DateTimeKind.Local);
            foreach (Act act in Acts)
            {
                DateTime start = _jan1st1970.AddMilliseconds(act.start);
                DateTime end = _jan1st1970.AddMilliseconds(act.end);
                Activity activity = new Activity
                {
                    FK_ProjectId = id,
                    Name = act.name,
                    StartDate = start,
                    EndDate = end,
                    EstDuration = act.duration,
                    FK_TeamId = 1,
                    Progress = 0,
                    Status = Status.OnHold,
                    Priority = Priority.Medium
                };
                unitOfWork.Tasks.Add(activity);
            }

            return View();
        }

    }
}