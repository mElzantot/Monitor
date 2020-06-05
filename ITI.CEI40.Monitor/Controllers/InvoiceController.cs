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
    public class InvoiceController : Controller
    {
        private IUnitOfWork unitOfWork;
        public InvoiceController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index_Income(int id)
        {
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                Invoices = unitOfWork.Invoices.GetIncomeByProjectId(id).ToList(),
                ProjectId = id,
                Project = unitOfWork.Projects.GetById(id)
            };
            TempData["InvoiceType"] = 1;
            return View(invoiceVM);
        }

        public IActionResult Index_Expense(int id)
        {
            InvoiceViewModel invoiceVM = new InvoiceViewModel
            {
                Invoices = unitOfWork.Invoices.GetExpensesByProjectId(id).ToList(),
                ProjectId = id,
                Project = unitOfWork.Projects.GetById(id)
            };
            TempData["InvoiceType"] = 0;
            return View("Index_Income", invoiceVM);
        }

        [HttpPost]
        public IActionResult AddInvoice(Invoice invoice, List<InvoiceItem> invoiceItems)
        {
            var s = Response;
            if (ModelState.IsValid)
            {
                Invoice newinvoice = unitOfWork.Invoices.Add(invoice);
                foreach (var item in invoiceItems)
                {
                    item.FK_InvoiceId = newinvoice.Id;
                    var DBitem = unitOfWork.InoviceItems.Add(item);
                }
                return PartialView("_InvoicePatrialView", newinvoice);
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
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}