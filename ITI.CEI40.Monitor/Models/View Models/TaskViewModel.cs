﻿using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class TaskViewModel
    {
        public string taskDescription { get; set; }

        public int TaskId { get; set; }

        public List<SubTask> SubTasks { get; set; }
        public Activity Task { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
