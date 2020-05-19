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
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
            if (name != null && unitOfWork.Departments.FindByName(name) == null)
            {
                var newDept = new Department
                {
                    Name = name

                };
                newDept = unitOfWork.Departments.Add(newDept);
                newDept = unitOfWork.Departments.GetDeptWithTeamsandManager(newDept.Id);
                return PartialView("_DepartmentPartial", newDept);
            }
            return null;

        }



    }
}