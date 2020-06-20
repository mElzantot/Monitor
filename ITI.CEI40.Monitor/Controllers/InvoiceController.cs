using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Hubs;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ITI.CEI40.Monitor.Controllers
{
    public class InvoiceController : Controller
    {
        private IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationsHub> hubContext;

        public InvoiceController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IHubContext<NotificationsHub> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hubContext = hubContext;

        }


        public IActionResult ControlInvoice()
        {
            List<Project> projects = unitOfWork.Projects.GetAll().ToList();
            return View(projects);
        }

        public IActionResult Index_Income(int id)
        {
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                Invoices = unitOfWork.Invoices.GetIncomeByProjectId(id).ToList(),
                ProjectId = id,
                Project = unitOfWork.Projects.GetById(id),
                CurrentUser = userManager.GetUserId(User)
            };
            TempData["InvoiceType"] = 1;
            return View("Index", invoiceVM);
        }

        public IActionResult Index_Expense(int id)
        {
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                Invoices = unitOfWork.Invoices.GetExpensesByProjectId(id).ToList(),
                ProjectId = id,
                Project = unitOfWork.Projects.GetById(id),
                CurrentUser = userManager.GetUserId(User)
            };
            TempData["InvoiceType"] = 0;
            return View("Index", invoiceVM);
        }

        [HttpPost]
        public IActionResult AddInvoice(Invoice invoice, List<InvoiceItem> invoiceItems)
        {
            Project project = unitOfWork.Projects.GetById(invoice.FK_ProjectId);

            if (ModelState.IsValid)
            {
                Invoice newinvoice = unitOfWork.Invoices.Add(invoice);
                foreach (var item in invoiceItems)
                {
                    item.FK_InvoiceId = newinvoice.Id;
                    var DBitem = unitOfWork.InoviceItems.Add(item);
                }
                Invoice newaddedinvoice = unitOfWork.Invoices.GetInvoiceWithItems(newinvoice.Id);

                // update project income & outcome
                if (newinvoice.InvoicesType == Entities.Enums.InvoicesType.Income)
                {
                    SetProjectIncome(newinvoice.FK_ProjectId);
                }
                else if (newinvoice.InvoicesType == Entities.Enums.InvoicesType.Expenses)
                { SetProjectOutcome(newinvoice.FK_ProjectId); }

                // notifications
                string messege = $"The finance department has added a new invoice to project *{project.Name}=* at *{DateTime.Now}=*";
                SendNotification(messege, project.FK_Manager);

                return PartialView("_InvoicePartialView", newaddedinvoice);
            }
            return null;
        }

        [HttpGet]
        public IActionResult GetInvoiceWithItems(int id)
        {
            var d = unitOfWork.Invoices.GetInvoiceWithItems(id);
            return PartialView("_InvoiceModal", d);
        }

        [HttpPost]
        public bool Delete(int id)
        {
            Invoice inv = unitOfWork.Invoices.GetById(id);
            if (inv != null)
            {
                List<InvoiceItem> invoiceItems = unitOfWork.InoviceItems.GetAllBuInvoiceId(id).ToList();
                if (invoiceItems.Count > 0)
                {
                    foreach (var item in invoiceItems)
                    {
                        unitOfWork.InoviceItems.Delete(item.Id);
                    }
                }
                unitOfWork.Invoices.Delete(id);

                // update project income & outcome
                if (inv.InvoicesType == Entities.Enums.InvoicesType.Income)
                {
                    SetProjectIncome(inv.FK_ProjectId);
                }
                else if (inv.InvoicesType == Entities.Enums.InvoicesType.Expenses)
                { SetProjectOutcome(inv.FK_ProjectId); }
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool SetProjectIncome(int id)
        {
            float totalincome = 0;
            List<Invoice> incomeinvoices = unitOfWork.Invoices.GetIncomeByProjectId(id).ToList();
            if (incomeinvoices != null && incomeinvoices.Count > 0)
            {
                foreach (Invoice income in incomeinvoices)
                {
                    totalincome += income.Value;
                }
                Project project = unitOfWork.Projects.GetById(id);
                project.Income = totalincome;
                unitOfWork.Projects.Edit(project);
                return true;
            }
            else { return false; }
        }

        public bool SetProjectOutcome(int id)
        {
            float totaloutcome = 0;
            List<Invoice> outcomeinvoices = unitOfWork.Invoices.GetExpensesByProjectId(id).ToList();
            if (outcomeinvoices != null && outcomeinvoices.Count > 0)
            {
                foreach (Invoice outcome in outcomeinvoices)
                {
                    totaloutcome += outcome.Value;
                }
                Project project = unitOfWork.Projects.GetById(id);
                project.Outcome = totaloutcome;
                unitOfWork.Projects.Edit(project);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendNotification(string messege, params string[] usersId)
        {
            Notification Notification = new Notification
            {
                messege = messege,
                seen = false
            };
            Notification Savednotification = unitOfWork.Notification.Add(Notification);

            for (int i = 0; i < usersId.Length; i++)
            {
                NotificationUsers notificationUsers = new NotificationUsers
                {
                    NotificationId = Savednotification.Id,
                    userID = usersId[i]
                };
                unitOfWork.NotificationUsers.Add(notificationUsers);

                //---------Send Notification to Employee
                hubContext.Clients.User(usersId[i]).SendAsync("newNotification", messege, false, Savednotification.Id);
            }

        }

    }
}