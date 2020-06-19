using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ActivityViwewModel
    {
        public int ProjectId { get; set; }
        public List<Act> Acts { set; get; }
        public List<Activity> Activities { set; get; }
        public List<int> reDbId { set; get; }
        public List<DeletedDependecy> reDep { set; get; }
        public List<Department> Departments { set; get; }
        public List<Holiday> Holidays { set; get; }
    }

    public class Act
    {
        public int id { get; set; }
        public string name { get; set; }
        public long start { get; set; }
        public long end { get; set; }
        public int duration { get; set; }
        public int progress { get; set; }
        public int dbId { get; set; }
        public int assignee { get; set; }
        public Dependecy Dependecy { get; set; }
        public List<Dependecy> Dependecies { get; set; }

    }

    public class Dependecy
    {
        public int id { get; set; }
        public int lag { get; set; }
    }
    public class DeletedDependecy
    {
        public int followed { get; set; }
        public int following { get; set; }
    }
}
