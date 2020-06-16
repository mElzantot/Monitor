using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class CompletedProjectsViewModel
    {
        public CompletedProjectsViewModel(Project project)
        {
            this.ProjectId = project.ID;
            this.Name = project.Name;
            this.Owner = project.Owner;
            if (project.StartDate != null)
            this.StartDate = project.StartDate.Value.ToString("dd/mm/yyyy")  ;
            else
                this.StartDate = "";
            this.EndDate = project.EndDate.Value.ToString("dd/mm/yyyy");
            this.Income = project.Income;
            this.Outcome = project.Outcome;
            this.WorkingHours = project.WorkingHrs;
            this.Profit = 100 * ( (this.Income / this.Outcome)-1);
        }

        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public float WorkingHours { get; set; }

        public float Income { get; set; }

        public float Outcome { get; set; }

        public float Profit { get; set; }

    }
}
