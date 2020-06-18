using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models
{
    public class TeamChartViewModel
    {
        public List<string> Names { get; set; }
        public List<List<float>> Values { get; set; }
        public List<string> EngIds { get; set; }
    }
}
