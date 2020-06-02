using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class TeamController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public TeamController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ViewTeams(int id,string Name)
        {
            TeamViewModel TeamVm = new TeamViewModel
            {
                teams = unitOfWork.Teams.getTeamsinsideDept(id).ToList(),
                FK_DepartmentId = id,
                DepName = Name
            };
            return PartialView("_ViewTeamsPartial",TeamVm);
        }

        [HttpPost]
        public IActionResult AddTeam(Team team)
        {
            if (team.Name != null && unitOfWork.Teams.FindByName(team.Name) == null)
            {
                var newTeam = new Team
                {
                    Name = team.Name,
                    FK_DepartmentId = team.FK_DepartmentId
                };
                newTeam = unitOfWork.Teams.Add(newTeam);
                newTeam = unitOfWork.Teams.GetTeamWithEngineersandLeader(newTeam.Id);
                return PartialView("_TeamPartialView", newTeam);
            }

            //------Need Solution
            return null;
        }

        [HttpPost]
        public bool DeleteTeam(int id)
        {
            var team = unitOfWork.Teams.GetTeamWithTasksAndEngineers(id);
            if (team != null && team.Engineers.Count() == 0 && team.Tasks.Count() == 0)
            {
                return unitOfWork.Teams.Delete(id);
            }
            return false;
        }

    }
}