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
                Activities = unitOfWork.Tasks.GetByProjectId(id).OrderBy(t => t.ViewOrder).ToList()
            };
            return View(activityVM);
        }

        [HttpPost]
        public IActionResult AddActivities(int id, List<Act> Acts, List<int> reDbId, List<DeletedDependecy> reDep)
        {
            var x = Response;
            // adding
            DateTime _jan1st1970 = new DateTime(1970, 1, 1, 23, 59, 59, DateTimeKind.Local);
            foreach (Act act in Acts)
            {
                DateTime start = _jan1st1970.AddMilliseconds(act.start);
                DateTime end = _jan1st1970.AddMilliseconds(act.end);
                // adding all the new activities
                if (act.dbId == 0)
                {
                    Activity activity = new Activity
                    {
                        FK_ProjectId = id,
                        ViewOrder = act.id,
                        Name = act.name,
                        StartDate = start,
                        EndDate = end,
                        EstDuration = act.duration,
                        Progress = 0,
                        Status = Status.OnHold,
                        Priority = Priority.Medium
                    };
                    unitOfWork.Tasks.Add(activity);
                }
                // Edit the exisiting ones
                else
                {
                    Activity oldAct = unitOfWork.Tasks.GetById(act.dbId);
                    oldAct.Id = act.dbId;
                    oldAct.FK_ProjectId = id;
                    oldAct.ViewOrder = act.id;
                    oldAct.Name = act.name;
                    oldAct.StartDate = start;
                    oldAct.EndDate = end;
                    oldAct.EstDuration = act.duration;
                    oldAct.Progress = 0;
                    oldAct.Status = Status.OnHold;
                    oldAct.Priority = Priority.Medium;

                    unitOfWork.Tasks.Edit(oldAct);
                }
            }
            // managing the related dependencies
            foreach (Act act in Acts)
            {
                // adding all the new dependencies related to the new activities (from the independent view)
                if (act.Dependecies != null && act.dbId == 0)
                {
                    foreach (var dep in act.Dependecies)
                    {
                        Activity actToFollow = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.id);
                        Activity followingAct = unitOfWork.Tasks.GetByProIdAndViewOrder(id, dep.id);
                        Dependencies dependency = new Dependencies
                        {
                            ActivityToFollowId = actToFollow.Id,
                            FollowingActivityId = followingAct.Id,
                            Lag = dep.lag
                        };
                        unitOfWork.Dependency.Add(dependency);

                    }
                }
                // add the new dependencies related to old activities (from the dependent view)
                if (act.Dependecy != null && act.dbId != 0 && reDep.Exists(d=>d.following == act.id))
                {
                    Activity actToFollow = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.Dependecy.id);
                    Activity followingAct = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.id);
                    Dependencies dependency = new Dependencies
                    {
                        ActivityToFollowId = actToFollow.Id,
                        FollowingActivityId = followingAct.Id,
                        Lag = act.Dependecy.lag
                    };
                    unitOfWork.Dependency.Add(dependency);
                }
            }
            // deleting the deleted dependencies
            foreach (var deleted in reDep)
            {
                Activity actToFollow = unitOfWork.Tasks.GetByProIdAndViewOrder(id, deleted.followed);
                Activity followingAct = unitOfWork.Tasks.GetByProIdAndViewOrder(id, deleted.following);
                Dependencies oldDep = unitOfWork.Dependency.GetById(actToFollow.Id, followingAct.Id);
                if (oldDep != null)
                {
                    unitOfWork.Dependency.Delete(oldDep.ActivityToFollowId, oldDep.FollowingActivityId);
                }
            }
            // deleting the deleted activities
            foreach (var actId in reDbId)
            {
                unitOfWork.Tasks.Delete(actId);
            }

            return RedirectToAction(nameof(Index),id);
        }

    }
}