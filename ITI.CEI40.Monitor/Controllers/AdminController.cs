using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager, IUnitOfWork unitofwork)
        {
            this.roleManager = roleManager;
            this.unitofwork = unitofwork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            AdminViewModel AdminVm = new AdminViewModel
            {
                Departments = unitofwork.Departments.GetAllDeptWithTeamsandManagers()

            };

            return View(AdminVm);
        }


        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddDepartment(string name)
        {
            if (name != null && unitofwork.Departments.FindByName(name) == null)
            {
                var newDept = new Department
                {
                    Name = name

                };
                newDept = unitofwork.Departments.Add(newDept);
                newDept = unitofwork.Departments.GetDeptWithTeamsandManager(newDept.Id);
                return PartialView("_AdminIndexPartial", newDept);
            }
            return null;

        }


        public JsonResult GetDepartment(int id)
        {
            var department = unitofwork.Departments.GetDeptWithManager(id);
            var manager = department.Manager;
            return Json(department);
        }


        [HttpGet]
        public IActionResult EditDepartment()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddTeam()
        {
            TeamViewModel teamVm = new TeamViewModel
            {
                Departments = unitofwork.Departments.GetAll().ToList()
            };

            return View(teamVm);
        }

        [HttpPost]
        public IActionResult AddTeam(Team team)
        {
            if (ModelState.IsValid)
            {
                var newTeam = new Team
                {
                    Name = team.Name,
                    FK_DepartmentId = team.FK_DepartmentId
                };

                var entity = unitofwork.Teams.Add(newTeam);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TeamViewModel teamVm = new TeamViewModel
                {
                    Departments = unitofwork.Departments.GetAll().ToList()
                };
                return View(teamVm);
            }

        }



        [HttpGet]
        public IActionResult AddEmployee()
        {

            EmployeeViewModel empVm = new EmployeeViewModel
            {
                teams = unitofwork.Teams.GetAll().ToList(),
                Roles = roleManager.Roles.ToList<IdentityRole>()
            };

            return View(empVm);
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
                    FK_TeamID = employee.FK_TeamId
                };

                var result = await userManager.CreateAsync(newEmp, employee.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newEmp, employee.role);
                }

                return RedirectToAction("Index", "Home");
            }

            EmployeeViewModel empVm = new EmployeeViewModel
            {
                teams = unitofwork.Teams.GetAll().ToList()
            };

            return View(empVm);

        }


        [HttpGet]
        public IActionResult EditEmployee()
        {
            return View();
        }


    }
}