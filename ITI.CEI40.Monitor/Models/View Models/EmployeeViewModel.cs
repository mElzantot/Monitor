using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class EmployeeViewModel
    {

        [Required]
        [Display(Name =  "Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int? DepId { get; set; }

        [Display(Name = "Department Name")]
        public string DepName { get; set; }

        public int? TeamId { get; set; }
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        public List<Department> Departments { get; set; }
        public List<Team> Teams { get; set; }


        public float TotalEvaluation { get; set; }

        [Required]
        [Range(0, 100000)]
        public float Salary { get; set; }

        public List<ApplicationUser> Employees { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
