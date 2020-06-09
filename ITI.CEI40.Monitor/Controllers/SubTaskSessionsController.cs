using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class SubTaskSessionsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public SubTaskSessionsController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }


        [Authorize(Roles = "Engineer")]
        [HttpGet]
        public IActionResult EmployeeTimeSheet()
        {
            List<SubTaskSession> EmpSessions = unitOfWork.SubTaskSessions.GetTimeSheetForEmp(userManager.GetUserId(HttpContext.User)).ToList();

            return View(EmpSessions);
        }

        //---------Daily Report for Team Leader



    }
}