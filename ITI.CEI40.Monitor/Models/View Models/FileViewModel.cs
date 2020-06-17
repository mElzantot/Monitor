using ITI.CEI40.Monitor.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class FileViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string comment { get; set; }

        [Required]
        public IFormFile file { get; set; }

        [Required]
        public int taskId { get; set; }

        [Required]
        public int subTaskId { get; set; }


    }
}
