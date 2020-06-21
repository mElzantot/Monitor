using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class EmployeeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHubContext<NotificationsHub> hubContext;
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public EmployeeController(RoleManager<IdentityRole> roleManager, IHubContext<NotificationsHub> hubContext,
           UserManager<ApplicationUser> userManager, IUnitOfWork unitofwork)
        {
            this.roleManager = roleManager;
            this.hubContext = hubContext;
            this.unitofwork = unitofwork;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ViewEmployees()
        {
            var EmploeeVm = new EmployeeViewModel
            {
                Departments = unitofwork.Departments.GetAll().ToList(),
                Employees = unitofwork.Engineers.GetAll().ToList()
            };
            return View(EmploeeVm);
        }

        [HttpGet]
        public IActionResult ViewFilteredEmployees([FromForm]int teamId)
        {
            List<ApplicationUser> Employees = unitofwork.Engineers.GetEngineersInsideTeam(teamId).ToList();

            return PartialView("_teamsEmployeePartialView", Employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                var newEmp = new ApplicationUser
                {
                    UserName = employee.UserName,
                    Email = employee.Email,
                    FK_TeamID = employee.TeamId,
                    SalaryRate = employee.Salary / (30 * 8)
                };

                var result = await userManager.CreateAsync(newEmp, employee.Password);

                if (result.Succeeded)
                {
                    return PartialView("_EmployeePartialView", newEmp);
                }
            }

            //-------Not Acceptable ( Need solution ) ------//
            return null;

        }

        #region Edit Roles for Employees

        [HttpGet]
        public async Task<IActionResult> EditEmployeeRoles(string id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Emp = await userManager.FindByIdAsync(id);
                //---------Get The Roles before Updating
                var EmpCurrentRoles = await userManager.GetRolesAsync(Emp);
                List<IdentityRole> ExistingRoles = roleManager.Roles.ToList();
                EmpRolesViewModel EmpRolVM = new EmpRolesViewModel() { EmpId = id };
                bool checkFlag = false;
                foreach (IdentityRole item in ExistingRoles)
                {
                    checkFlag = false;
                    for (int i = 0; i < EmpCurrentRoles.Count && !checkFlag; i++)
                    {
                        if (item.ToString() == EmpCurrentRoles[i])
                        {
                            checkFlag = true;
                            EmpRolVM.EmpRoles.Add(new RolesModel() { role = item, IsSelected = true });
                        }
                    }
                    if (!checkFlag)
                    {
                        EmpRolVM.EmpRoles.Add(new RolesModel() { role = item, IsSelected = false });
                    }
                }
                return PartialView("_EmploeeRolesPartialView", EmpRolVM);
            }
            return null;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<JsonResult> EditEmployeeRoles(EmpRolesViewModel EmpRolVM)
        {

            ApplicationUser Emp = await userManager.FindByIdAsync(EmpRolVM.EmpId);
            var EmpCurrentRoles = await userManager.GetRolesAsync(Emp);
            if (EmpCurrentRoles != null && EmpCurrentRoles.Count > 0)
            {
                var removeFromRoles = await userManager.RemoveFromRolesAsync(Emp, EmpCurrentRoles);
            }

            foreach (var item in EmpRolVM.EmpRoles)
            {
                string roleName = item.role.ToString();
                if (item.IsSelected)
                {
                    var addedToRoles = await userManager.AddToRoleAsync(Emp, item.role.ToString());
                }
            }

            var isDeptManager = userManager.IsInRoleAsync(Emp, Roles.DepartmentManager.ToString());

            if (isDeptManager.Result)
            {
                Emp.FK_TeamID = null;
                Emp.Team = null;
                var result = await userManager.UpdateAsync(Emp);
            }

            return Json(Emp); ;
        }

        #endregion


        #region Edit Employee Data

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            ApplicationUser Emp = await userManager.FindByIdAsync(id);
            if (Emp != null)
            {
                EditEmpViewModel EmpVm = new EditEmpViewModel
                {
                    EmpId = Emp.Id,
                    Salary = Emp.SalaryRate * 30 * 8,
                    EmpName = Emp.UserName,
                    EMail = Emp.Email,
                    DepName = string.Empty,
                    Departments = unitofwork.Departments.GetAll().ToList()
                    //Teams = unitofwork.Teams.GetAll().ToList()
                };

                return PartialView("_EditEmpoyeePartialView", EmpVm);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetTeamsforEdit(int id)
        {
            List<Team> teams;
            try
            {
                teams = unitofwork.Teams.getTeamsinsideDept(id).ToList();

            }
            catch (Exception)
            {

                throw;
            }
            return Json(teams);
        }



        [HttpPost]
        public async Task<JsonResult> EditEmployee(EditEmpViewModel EmpVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser emp = await userManager.FindByIdAsync(EmpVM.EmpId);
                if (EmpVM.TeamId != 0)
                {
                    emp.FK_TeamID = EmpVM.TeamId;
                }

                emp.Email = EmpVM.EMail;
                emp.SalaryRate = EmpVM.Salary / (30 * 8);

                var result = await userManager.UpdateAsync(emp);
                if (result.Succeeded)
                {
                    return Json(EmpVM);
                }
            }
            return null;
        }


        #endregion

        [HttpGet]
        public async Task<JsonResult> AssignManager(string role)
        {
            var Employees = await userManager.GetUsersInRoleAsync(role);
            return Json(Employees);
        }


        [HttpPost]
        //------Id ----> the Department or team Id 
        //-------EmpID is ---> Manager or Leader Id
        //----Bool is to differ between Department and Team 
        //------True == Department & False == Team
        public async Task<JsonResult> AssignManager(int id, string EmpId, bool IsDept)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Employee = await userManager.FindByIdAsync(EmpId);
                if (IsDept)
                {
                    Department dept = unitofwork.Departments.GetById(id);
                    dept.FK_ManagerID = EmpId;
                    unitofwork.Complete();
                    // notifications
                    string messege = $"Congartulations you have been assigned as a Department manager to *{dept.Name}=* Department at *{DateTime.Now}=*";
                    SendNotification(messege, EmpId);
                    return Json(new { complete = true, DeptId = id, ManagerName = Employee.UserName });
                }
                else
                {
                    Team team = unitofwork.Teams.GetById(id);
                    team.FK_TeamLeaderId = EmpId;
                    int NoOfRows = unitofwork.Complete();
                    // notifications
                    string messege = $"Congartulations you have been assigned as a Leader to *{team.Name}=* Team at *{DateTime.Now}=*";
                    SendNotification(messege, EmpId);
                    return Json(new { complete = true, teamtId = id, ManagerName = Employee.UserName });

                }
            }
            return Json(new { Complete = false });
        }


        //-----------Employee TimeSheet
        [Authorize(Roles = "Engineer")]
        [HttpGet]
        public IActionResult EmployeeTimeSheet()
        {
            List<SubTaskSession> EmpSessions = unitofwork.SubTaskSessions.GetTimeSheetForEmp(userManager.GetUserId(HttpContext.User)).Reverse().ToList();

            return View(EmpSessions);
        }


        public void SendNotification(string messege, params string[] usersId)
        {
            Notification Notification = new Notification
            {
                messege = messege,
                seen = false
            };
            Notification Savednotification = unitofwork.Notification.Add(Notification);

            for (int i = 0; i < usersId.Length; i++)
            {
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = usersId[i]
                };
                unitofwork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Employee
                hubContext.Clients.User(usersId[i]).SendAsync("newNotification", messege, false, Savednotification.Id);
            }

        }

    }
}