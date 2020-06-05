using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class InvoiceItemManager : Reposiotry<ApplicationDbContext, InvoiceItem>, IInoviceItemManger
    {
        public InvoiceItemManager(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<InvoiceItem> GetAllBuInvoiceId(int id)
        {
            return set.Where(i => i.FK_InvoiceId == id);
        }
    }
}
