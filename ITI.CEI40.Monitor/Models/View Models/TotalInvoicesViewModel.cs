using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class TotalInvoicesViewModel
    {
        public int Year { get; set; }
        
        public int Month { get; set; }

        public float Expenses { get; set; }
        
        public float Sales { get; set; }
    }
}
