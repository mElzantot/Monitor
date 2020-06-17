using ITI.CEI40.Monitor.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class Notification
    {
        public Notification()
        {
            this.NotificationUsers = new HashSet<NotificationUsers>();
            this.seen = false;
        }


        [Key]
        public int Id { get; set; }

        //[Required]
        //[ForeignKey("Sender")]
        //public string FK_SenderId { get; set; }

        //public ApplicationUser Sender { get; set; }

        //public string TaskName { get; set; }

        //public NotificationType Action { get; set; }

        //public string ActionValue { get; set; }

        public ICollection<NotificationUsers> NotificationUsers { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        //public DateTime NotifTime { get; set; }

        [Required]
        public string messege { get; set; }

        public bool seen { get; set; }
    }
}
