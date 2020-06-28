using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<NotificationsHub> hubContext;

        public DepartmentController(IUnitOfWork unitOfWork, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult ViewDepartments()
        {
            //------------Get Departments to Display them
            DepartmentViewModel DeptVm = new DepartmentViewModel
            {
                Departments = unitOfWork.Departments.GetAllDeptWithTeamsandManagers()

            };

            return View(DeptVm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool DeleteDepartment(int id)
        {
            var department = unitOfWork.Departments.GetDeptWithTeamsAndProjects(id);
            //--------------Check if the Department has any Teams inside it
            //-------------check if the department has assigned projects
            if (department != null && department.Teams.Count() == 0 && department.DepartmentProjects.Count() == 0)
            {
                return unitOfWork.Departments.Delete(id);
            }
            return false;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddDepartment(string name)
        {
            //-----------Check that the Name of the Department isn`t exist
            Department ExistingDept = unitOfWork.Departments.FindByName(name);
            //------If the Name is unique Create New Department
            if (ExistingDept == null)
            {
                var newDept = new Department
                {
                    Name = name
                };
                //----------Add Dept to DB
                newDept = unitOfWork.Departments.Add(newDept);

                #region notification test
                //string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //string messege = $"*{HttpContext.User.Identity.Name}=* -Admin- has added *{name}=* as a new Department at *{DateTime.Now}=*.";
                //Notification Notification = new Notification
                //{
                //    messege = messege,
                //    seen = false
                //};
                //Notification Savednotification = unitOfWork.Notification.Add(Notification);
                //NotificationUsers notificationUsers = new NotificationUsers
                //{
                //    NotificationId = Savednotification.Id,
                //    userID = currentUserId
                //};
                //unitOfWork.NotificationUsers.Add(notificationUsers);

                //hubContext.Clients.All.SendAsync("newNotification", messege, false, Savednotification.Id);
                #endregion

                // List<Department> depts = unitOfWork.Departments.GetAllDeptWithManagers().ToList();
                return PartialView("_DepartmentPartial", newDept);
            }
            return null;

        }

    }
}