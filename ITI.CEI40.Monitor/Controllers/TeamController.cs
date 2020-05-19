using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.CEI40.Monitor.Data;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Models.View_Models;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class TeamController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TeamController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult ViewTeams(int id)
        {
            TeamViewModel TeamVm = new TeamViewModel
            {
                teams = unitOfWork.Teams.GetAllWithAttributes(id).ToList(),
                FK_DepartmentId = id,
            };
            return View(TeamVm);
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