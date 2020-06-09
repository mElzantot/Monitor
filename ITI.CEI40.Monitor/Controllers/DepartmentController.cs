using System;
using System.Collections.Generic;
using System.Linq;
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

        public DepartmentController(IUnitOfWork unitOfWork, IHubContext<NotificationsHub> hubContext )
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }


        public IActionResult ViewDepartments()
        {
            DepartmentViewModel DeptVm = new DepartmentViewModel
            {
                Departments = unitOfWork.Departments.GetAllDeptWithTeamsandManagers()

            };

            return View(DeptVm);
        }

        [HttpPost]
        public bool DeleteDepartment(int id)
        {
            var department = unitOfWork.Departments.GetDeptWithTeamsAndProjects(id);
            if (department != null && department.Teams.Count() == 0 && department.DepartmentProjects.Count() == 0)
            {
                return unitOfWork.Departments.Delete(id);
            }
            return false;
        }

        [HttpPost]
        public IActionResult AddDepartment(string name)
        {

            Department ExistingDept = unitOfWork.Departments.FindByName(name);
            if (ExistingDept == null)
            {
                var newDept = new Department
                {
                    Name = name
                };
                newDept = unitOfWork.Departments.Add(newDept);
                // List<Department> depts = unitOfWork.Departments.GetAllDeptWithManagers().ToList();
                return PartialView("_DepartmentPartial", newDept);
            }
            return null;

        }

    }
}