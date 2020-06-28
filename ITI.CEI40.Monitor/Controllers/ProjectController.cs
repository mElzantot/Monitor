using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using ITI.CEI40.Monitor.Models;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{

    [Authorize(Roles = "ProjectManager")]
    public class ProjectController : Controller
    {

        private readonly IUnitOfWork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.unitofwork = unitOfWork;
            this.userManager = userManager;
        }


        //  return view which contain list of projects which managed by projrct manager
        //  in this view the project manager can also create new project , delete and archive one.
        public IActionResult Index()
        {
            ProjectViewModel projectView = new ProjectViewModel
            {
                Projects = unitofwork.Projects.GetRunningProjects(userManager.GetUserId(HttpContext.User)),
            };
            return View("_CreateProject", projectView);
        }

        // View contains details of each Project (List of tasks in the project)
        [HttpGet]
        public IActionResult Details(int Id)
        {
            ActivityViewModel activityVM = new ActivityViewModel();
            activityVM.Tasks = unitofwork.Tasks.GetProjectTasksWithDep(Id).ToList();
            return View(activityVM);
        }

        // Submit the project when it finished
        public void Arcive(int PrjId,int status)
        {
            Project project = unitofwork.Projects.GetById(PrjId);
            project.Status = (Status)status;
            unitofwork.Projects.Edit(project);
        }

        // Add new project
        [HttpPost]
        public JsonResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                project.FK_Manager = userManager.GetUserId(HttpContext.User);
                project = unitofwork.Projects.Add(project);
                return Json(project);
            }
            else
            {
                return null;
            }
        }

        // open right section to see the details of the project
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Project project = unitofwork.Projects.GetById(id);
            if (project != null)
            {
                return PartialView("_FormPartial", project);
            }
            else
            {
                return null;
            }
        }

        //[HttpPost]
        //public JsonResult Edit(Project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        unitofwork.Projects.Edit(project);
        //        return Json(project);
        //    }
        //    else
        //    {
        //        return Json(project);
        //    }
        //}

        //Get completed projects as archive 
        [HttpGet]
        public IActionResult CompletedProjects()
        {
            //--------Start Date will be the start date of first task of project
            var compProjects = unitofwork.Projects.GetCompletedProjects(userManager.GetUserId(HttpContext.User)).ToList();
            List<CompletedProjectsViewModel> CompletedProjectsVM = new List<CompletedProjectsViewModel>();
            foreach (var item in compProjects)
            {
                CompletedProjectsViewModel compProject = new CompletedProjectsViewModel(item);
                CompletedProjectsVM.Add(compProject);
            }

            return View(CompletedProjectsVM);
        }


        public IActionResult DisplayProjects()
        {
            var project = unitofwork.Projects.GetAllProjects().ToList();
            return View(project);
        }

        // open dashboard for the project
        [HttpGet]
        public IActionResult DashBoard(int projId)
        {
            DashboardViewModel dashboard = new DashboardViewModel
            {
                Tasks = unitofwork.Tasks.GetActivitiesFromProject(projId).ToList(),
                TotalInvoices = TotalInvoices(projId)
            };
            return View("DashBoard", dashboard);
        }


        //Get Project daily report
        public IActionResult ProjectDailyReport(int Id)
        {
            Project project = unitofwork.Projects.GetProjectForReport(Id);
            return View(project);
        }


        public IActionResult Archive()
        {
            var projects = unitofwork.Projects.Archive().ToList();
            return View(projects);
        }


        // create invoices data for the chart
        public List<TotalInvoicesViewModel> TotalInvoices(int id)
        {
            List<Invoice> ex_invoices = unitofwork.Invoices.GetExpensesByProjectId(id).ToList();
            List<Invoice> in_invoices = unitofwork.Invoices.GetIncomeByProjectId(id).ToList();
            List<TotalInvoicesViewModel> totalInvoices = new List<TotalInvoicesViewModel>();
            // add the expenses to the total invoices
            if (ex_invoices != null && ex_invoices.Count > 0)
            {
                foreach (var item in ex_invoices)
                {
                    TotalInvoicesViewModel totalInvoicesViewModel = new TotalInvoicesViewModel
                    {
                        Year = item.PaymentDate.Date.Year,
                        Month = item.PaymentDate.Date.Month,
                        Expenses = item.Value
                    };
                    totalInvoices.Add(totalInvoicesViewModel);
                }
            }
            // add the sales to the total invoices
            if (in_invoices != null && in_invoices.Count > 0)
            {
                foreach (var item in in_invoices)
                {
                    TotalInvoicesViewModel totalInvoicesViewModel = new TotalInvoicesViewModel
                    {
                        Year = item.PaymentDate.Date.Year,
                        Month = item.PaymentDate.Date.Month,
                        Sales = item.Value
                    };
                    totalInvoices.Add(totalInvoicesViewModel);
                }
            }

            List<TotalInvoicesViewModel> FinalInvoices = totalInvoices.GroupBy(i => new { i.Year, i.Month }).OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month).Select(i => new TotalInvoicesViewModel
            {
                Year = i.Key.Year,
                Month = i.Key.Month,
                Expenses = i.Sum(e => e.Expenses),
                Sales = i.Sum(e => e.Sales)
            }).ToList();

            return FinalInvoices;
        }
    }
}