using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public class DepartmentProjects
    {

        public Department Department { get; set; }

        public Project Project { get; set; }

        public int DepartmentID { get; set; }

        public int ProjectID { get; set; }

    }
}
