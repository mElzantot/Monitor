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
    public class EmployeeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public EmployeeController(RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager, IUnitOfWork unitofwork)
        {
            this.roleManager = roleManager;
            this.unitofwork = unitofwork;
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult ViewEmployees(int id)
        {
            if (unitofwork.Teams.GetById(id) != null)
            {
                var EmploeeVm = new EmployeeViewModel
                {
                    Employees = unitofwork.Engineers.GetEngineersInsideTeam(id).ToList(),
                    Roles = roleManager.Roles.ToList<IdentityRole>(),
                    FK_TeamId = id
                };
                return View(EmploeeVm);
            }

            return BadRequest();
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
                    return PartialView("_EmployeePartialView", newEmp);
                }
            }
            //-------Not Acceptable ( Need solution ) ------//
            return null;

        }

    }
}