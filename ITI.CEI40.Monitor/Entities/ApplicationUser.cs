using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{

    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        {
            this.SubTasks = new HashSet<SubTask>();
            this.ManagedProjects = new HashSet<Project>();
            this.SubTaskSessions = new HashSet<SubTaskSession>();
            this.SentNotifications = new HashSet<Notification>();
            this.NotificationUsers = new HashSet<NotificationUsers>();
            this.SentComments = new HashSet<Comment>();
        }

        public int? FK_TeamID { get; set; }
        public Team Team { get; set; }


        [DataType(DataType.Date)]
        public DataType AvailableTime { get; set; }

        //-------Do we really need this ?!
        public int Workload { get; set; }

        public float TotalEvaluation { get; set; }

        [Required]
        public float SalaryRate { get; set; }

        //---------Many to Many Relation
        public ICollection<SubTask> SubTasks { get; set; }
        public ICollection<Project> ManagedProjects { get; set; }
        public ICollection<SubTaskSession> SubTaskSessions { get; set; }
        public ICollection<Notification> SentNotifications { get; set; }
        public ICollection<NotificationUsers> NotificationUsers { get; set; }

        public ICollection<Comment> SentComments { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
    }
}
