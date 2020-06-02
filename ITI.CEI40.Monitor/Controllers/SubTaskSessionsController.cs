using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class SubTaskSessionsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public SubTaskSessionsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [Authorize(Roles = "Engineer")]
        [HttpGet]
        public IActionResult EmployeeTimeSheet(string EmpId)
        {
            List<SubTaskSession> EmpSessions = unitOfWork.SubTaskSessions.GetTimeSheetForEmp(EmpId).ToList();

            return View(EmpSessions);
        }

        //---------Daily Report for Team Leader



    }
}