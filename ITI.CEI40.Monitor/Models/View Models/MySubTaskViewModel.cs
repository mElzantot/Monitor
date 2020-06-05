using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class MySubTaskViewModel
    {
        public SubTask SubTask { get; set; }

        public ICollection<IFormFile> Files { get; set; }
    }
}
