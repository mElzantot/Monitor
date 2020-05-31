using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class EmpRolesViewModel
    {
        public EmpRolesViewModel()
        {
            this.EmpRoles = new List<RolesModel>();
        }
        public string EmpId { get; set; }
        public List<RolesModel> EmpRoles { get; set; }
    }
}
