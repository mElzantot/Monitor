using ITI.CEI40.Monitor.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    [Table("Invoices")]
    public class Invoice
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public float Value { get; set; }

        [DataType(DataType.Date)]
        [Column("Invoice Date", TypeName = "Date")]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [DataType(DataType.Date)]
        [Column("Payment Date", TypeName = "Date")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        public InvoicesType InvoicesType { get; set; }

        [ForeignKey("Project")]
        public int FK_ProjectId { get; set; }

        public Project Project { get; set; }

        [ForeignKey("applicationUser")]
        public string CurrentUser { get; set; }

        public ApplicationUser applicationUser { get; set; }

        public ICollection<InvoiceItem> invoiceItems { get; set; }
    }
}
