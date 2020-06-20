using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class DashboardViewModel
    {
        public List<Activity> Tasks { get; set; }

        public List<TotalInvoicesViewModel> TotalInvoices { get; set; }
    }
}
