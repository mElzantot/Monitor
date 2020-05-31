using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Models.View_Models
{
    public class RolesModel
    {
        public IdentityRole role { get; set; }

        public bool IsSelected { get; set; }
    }
}
