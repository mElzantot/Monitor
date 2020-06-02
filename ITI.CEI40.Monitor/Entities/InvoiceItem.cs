using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }

        [ForeignKey("Invoice")]
        public int FK_InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
