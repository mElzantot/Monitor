using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public static class SeedData
    {

        public static void Initialize(RoleManager<IdentityRole> roleManager)
        {
            var roles = Enum.GetNames(typeof(Roles));
            foreach (var item in roles)
            {
                if (!roleManager.RoleExistsAsync(item).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = item;
                    role.NormalizedName = item.ToUpper();
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

    }
}
