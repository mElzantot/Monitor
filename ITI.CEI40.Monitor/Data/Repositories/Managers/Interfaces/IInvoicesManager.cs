using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces
{
    public interface IInvoicesManager: IRepository<Invoice>
    {
        IEnumerable<Invoice> GetExpensesByProjectId(int projectId);
        IEnumerable<Invoice> GetIncomeByProjectId(int projectId);
        Invoice GetInvoiceWithItems(int id);

    }
}
