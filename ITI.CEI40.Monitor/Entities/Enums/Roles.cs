using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities.Enums
{
    public enum Roles
    {
        Admin,
        [Display(Name = "Department Manager")]
        DepartmentManager,
        [Display(Name = "Project Manager")]
        ProjectManager,
        [Display(Name = "Team Leader")]
        TeamLeader,
        Engineer,
        Finance
    }
}
