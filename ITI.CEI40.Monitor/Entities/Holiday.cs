using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Holiday
    {
        public int Id { get; set; }

        public string HolidayName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Holiday")]
        public DateTime HolidayDate { get; set; }
    }
}
