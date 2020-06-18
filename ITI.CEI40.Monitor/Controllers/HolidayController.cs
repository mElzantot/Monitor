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
    public class HolidayController : Controller
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        public HolidayController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            HolidayViewModel holidayViewModel = new HolidayViewModel
            {
                Holidays = unitOfWork.Holiday.GetAll().ToList()
            };
            return View(holidayViewModel);
        }

        [HttpPost]
        public IActionResult AddHoliday (Holiday holiday)
        {
            Holiday newholiday = unitOfWork.Holiday.Add(holiday);
            return PartialView("_HolidayPartialView", newholiday);
        }

        [HttpPost]
        public bool DeleteHoliday(int id)
        {
            Holiday holiday = unitOfWork.Holiday.GetById(id);
            if (holiday != null)
            {
                unitOfWork.Holiday.Delete(id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}