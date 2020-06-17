﻿using ITI.CEI40.Monitor.Entities;
//using ITI.CEI40.Monitor.Migrations;
using System;
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
        public List<int> Complexity { get; set; }
        public List<SubTask> LastTasks { get; set; }
        public string EngineerName { get; set; }

    }
}
