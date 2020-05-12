﻿using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class TeamViewModel
    {

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Department")]
        public int FK_DepartmentId { get; set; }

        public List<Department> Departments { get; set; }

    }
}
