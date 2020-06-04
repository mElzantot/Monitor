using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class ProjFilesModelView
    {
        public ProjFilesModelView()
        {
            
        }


        public Project Project { get; set; }

        public List<IFormFile> Files { get; set; }


    }
}
