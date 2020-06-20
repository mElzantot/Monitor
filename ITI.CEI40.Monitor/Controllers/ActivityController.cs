using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using ITI.CEI40.Monitor.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class ActivityController : Controller
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private IHubContext<NotificationsHub> hubContext;
        public ActivityController(IUnitOfWork unitOfWork, IHubContext<NotificationsHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
            this.userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            ActivityViwewModel activityVM = new ActivityViwewModel
            {
                ProjectId = id,
                Activities = unitOfWork.Tasks.GetByProjectId(id).OrderBy(t => t.ViewOrder).ToList(),
                Departments = unitOfWork.Departments.GetAll().ToList(),
                Holidays = unitOfWork.Holiday.GetAll().ToList()
            };
            return View(activityVM);
        }

        [HttpPost]
        public IActionResult AddActivities(int id, List<Act> Acts, List<int> reDbId, List<DeletedDependecy> reDep)
        {
            // to convert from js date object to c# date object
            DateTime _jan1st1970 = new DateTime(1970, 1, 1, 23, 59, 59, DateTimeKind.Local);

            // (1)deleting the deleted dependencies (before editing the tasks)
            foreach (var deleted in reDep)
            {
                Activity actToFollow = unitOfWork.Tasks.GetById(deleted.followed);
                Activity followingAct = unitOfWork.Tasks.GetById(deleted.following);
                Dependencies oldDep = null;
                if (actToFollow != null && followingAct != null)
                {
                    oldDep = unitOfWork.Dependency.GetById(actToFollow.Id, followingAct.Id);
                }
                if (oldDep != null)
                {
                    unitOfWork.Dependency.Delete(oldDep.ActivityToFollowId, oldDep.FollowingActivityId);
                }
            }

            // (2)edit the old tasks
            foreach (Act act in Acts)
            {
                DateTime start = _jan1st1970.AddMilliseconds(act.start);
                DateTime end = _jan1st1970.AddMilliseconds(act.end);
                // Edit the exisiting tasks
                if (act.dbId != 0 && !reDbId.Exists(d => d == act.dbId))
                {
                    Activity oldAct = unitOfWork.Tasks.GetById(act.dbId);
                    int? oldassign = oldAct.FK_DepID;
                    oldAct.Id = act.dbId;
                    oldAct.FK_ProjectId = id;
                    oldAct.ViewOrder = act.id;
                    oldAct.Name = act.name;
                    oldAct.StartDate = start;
                    oldAct.EndDate = end;
                    oldAct.EstDuration = act.duration;
                    oldAct.FK_DepID = act.assignee;
                    oldAct.Progress = 0;
                    //oldAct.Status = Status.Active;
                    oldAct.Priority = Priority.Medium;
                    if (act.assignee == 0) { oldAct.FK_DepID = null; }
                    else if (oldassign != act.assignee) // notify about the assignment
                    {
                        #region notifications 
                        string DepmanagerId = unitOfWork.Departments.GetById(act.assignee).FK_ManagerID;
                        string messege = $"{HttpContext.User.Identity.Name}* -Project Manager- has assigned a new task -*{act.name}*- to your department at *{DateTime.Now.Date}";

                        //--------Add Notification to DataBase
                        Notification Notification = new Notification
                        {
                            messege = messege,
                            seen = false
                        };
                        Notification Savednotification = unitOfWork.Notification.Add(Notification);
                        NotificationUsers notificationUsers = new NotificationUsers
                        {
                            NotificationId = Savednotification.Id,
                            userID = DepmanagerId
                        };
                        unitOfWork.NotificationUsers.Add(notificationUsers);

                        //---------Send Notification to Team
                        hubContext.Clients.User(DepmanagerId).SendAsync("newNotification", messege, false, Savednotification.Id);
                        #endregion
                    }
                    unitOfWork.Tasks.Edit(oldAct);
                }
                //else if (act.dbId != 0 && reDbId.Exists(d => d == act.dbId))
                //{
                //    Activity oldAct = unitOfWork.Tasks.GetById(act.dbId);
                //    oldAct.ViewOrder = -1; // to avoid conflict when getting the dependent task
                //}
            }

            // (3)Cancelling/Deleting the deleted activities
            foreach (var actId in reDbId)
            {
                //Activity activity = unitOfWork.Tasks.GetById(actId);
                //activity.Status = Status.Cancelled;
                //activity.ViewOrder = -1;  //to resolve the conflict in getting the tasks back
                //unitOfWork.Tasks.Edit(activity);

                unitOfWork.Tasks.Delete(actId);
            }

            // (4)adding the new tasks
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
                        FK_DepID = act.assignee,
                        Progress = 0,
                        Status = Status.OnHold,
                        Priority = Priority.Medium
                    };
                    if (act.assignee == 0) { activity.FK_DepID = null; }
                    else // notify about the assignment
                    {
                        #region  notifications 
                        string DepmanagerId = unitOfWork.Departments.GetById(act.assignee).FK_ManagerID;
                        string messege = $"{HttpContext.User.Identity.Name} -Project Manager- has assigned a new task -{act.name}- to your department at {DateTime.Now.Date}";

                        //--------Add Notification to DataBase
                        Notification Notification = new Notification
                        {
                            messege = messege,
                            seen = false
                        };
                        Notification Savednotification = unitOfWork.Notification.Add(Notification);
                        NotificationUsers notificationUsers = new NotificationUsers
                        {
                            NotificationId = Savednotification.Id,
                            userID = DepmanagerId
                        };
                        unitOfWork.NotificationUsers.Add(notificationUsers);

                        //---------Send Notification to Team
                        hubContext.Clients.User(DepmanagerId).SendAsync("newNotification", messege, false, Savednotification.Id);
                        #endregion
                    }
                    unitOfWork.Tasks.Add(activity);
                }
            }

            // (5)managing the related dependencies
            foreach (Act act in Acts)
            {
                #region oldWay
                // adding all the new dependencies related to the new activities (from the independent view)
                //if (act.Dependecies != null && act.dbId == 0)
                //{
                //    foreach (var dep in act.Dependecies)
                //    {
                //        Activity actToFollow = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.id);
                //        Activity followingAct = unitOfWork.Tasks.GetByProIdAndViewOrder(id, dep.id);
                //        Dependencies dependency = new Dependencies
                //        {
                //            ActivityToFollowId = actToFollow.Id,
                //            FollowingActivityId = followingAct.Id,
                //            Lag = dep.lag
                //        };
                //        unitOfWork.Dependency.Add(dependency);

                //    }
                //} 
                #endregion
                // add the dependencies related to activities (from the dependent view)
                if (act.Dependecy != null && (act.dbId == 0 || !reDep.Exists(d => d.following == act.dbId)))
                {
                    List<Activity> actsToFollow = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.Dependecy.id).ToList();
                    Activity actToFollow = GetFollowedActivity(actsToFollow, reDep);
                    List<Activity> followingActs = unitOfWork.Tasks.GetByProIdAndViewOrder(id, act.id).ToList();
                    Activity followingAct = GetFollowingActivity(followingActs, reDep);

                    Dependencies dependency = new Dependencies
                    {
                        ActivityToFollowId = actToFollow.Id,
                        FollowingActivityId = followingAct.Id,
                        Lag = act.Dependecy.lag
                    };
                    Dependencies OldDependency = unitOfWork.Dependency.GetById(dependency.ActivityToFollowId, dependency.FollowingActivityId);
                    if (OldDependency == null) { unitOfWork.Dependency.Add(dependency); }
                }
            }

            return RedirectToAction(nameof(SetProjectDates), new { id = id });
        }

        [Route("SetProjectDates/{id}")]
        public IActionResult SetProjectDates(int id)
        {
            // getting the activties of the project in database
            List<Activity> Acts = unitOfWork.Tasks.GetByProjectId(id).ToList();
            if (Acts.Count > 0 && Acts != null)
            {
                //setting the start and the end of the project
                DateTime FirstStart = Acts.First().StartDate, LastEnd = Acts.Last().EndDate;
                foreach (var act in Acts)
                {
                    if (DateTime.Compare(act.StartDate, FirstStart) < 0) { FirstStart = act.StartDate; }
                    if (DateTime.Compare(act.EndDate, LastEnd) > 0) { LastEnd = act.EndDate; }
                }
                Project project = unitOfWork.Projects.GetById(id);
                project.StartDate = FirstStart; project.EndDate = LastEnd;
                var res = unitOfWork.Projects.Edit(project);
                return RedirectToAction(nameof(Index), new { id = id });
            }
            else
            {
                return View("Error");
            }
        }

        // helper methods to get the right tasks (from the dependent view)
        public Activity GetFollowingActivity(List<Activity> activities, List<DeletedDependecy> reDep)
        {
            if (activities.Count == 1)
            {
                return activities[0];
            }
            foreach (Activity act in activities)
            {
                if (reDep.Exists(dbid => dbid.following != act.Id))
                {
                    return act;
                }
            }
            return null;
        }

        public Activity GetFollowedActivity(List<Activity> activities, List<DeletedDependecy> reDep)
        {
            if (activities.Count == 1)
            {
                return activities[0];
            }
            foreach (Activity act in activities)
            {
                if (reDep.Exists(dbid => dbid.followed != act.Id))
                {
                    return act;
                }
            }
            return null;
        }
    }
}