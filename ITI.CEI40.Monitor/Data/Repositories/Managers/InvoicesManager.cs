using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class InvoicesManager : Reposiotry<ApplicationDbContext, Invoice>, IInvoicesManager
    {
        public InvoicesManager(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Invoice> GetIncomeByProjectId(int projectId)
        {
            return set.Where(i => i.InvoicesType == InvoicesType.Income).Where(i => i.FK_ProjectId == projectId).Include(i=>i.Project);
        }

        public IEnumerable<Invoice> GetExpensesByProjectId(int projectId)
        {
            return set.Where(i => i.InvoicesType == InvoicesType.Expenses).Where(i => i.FK_ProjectId == projectId)
                .Include(i => i.Project);
        }

        public Invoice GetInvoiceWithItems(int id)
        {
            return set.Where(i => i.Id == id).Include(i => i.invoiceItems).FirstOrDefault();
        }
    }
}
