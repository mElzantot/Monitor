﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class EngineerChrtViewModel
    {
        public List<string> Months { get; set; }
        public List<int> Quality { get; set; }
        public List<int> Time { get; set; }
        public string EngineerName { get; set; }
        public List<int> SubMonth { get; set; }
    }
}