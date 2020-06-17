using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class InvoiceViewModel
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public Invoice Invoice { get; set; }
        public List<Invoice> Invoices { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public string CurrentUser { get; set; }
    }

}
