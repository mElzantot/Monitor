using ITI.CEI40.Monitor.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class SubTaskManager : Reposiotry<ApplicationDbContext, SubTask>, ISubTaskManager
    {
        public SubTaskManager(ApplicationDbContext context) : base(context)
        {

        }

        public SubTask GetClaims(int id)
        {
            return set.Where(st => st.Id == id).Include(st => st.Claims).FirstOrDefault();
        }

        public List<SubTask> GetSubTasksByTaskId(int taskId)
        {
            return set.Where(s => s.FK_TaskId == taskId).Include(s=>s.Task).Include(s => s.Engineer).ToList();
        }

        //----------Must Edit include engineering subtask
        public IEnumerable<SubTask> GetSubTasksFromTask(int taskId)
        {
            return set.Where(st => st.FK_TaskId == taskId).Include(s => s.Task).Include(s => s.Engineer).Include(t => t.SubTaskSession).ToList();
        }

        public IEnumerable<SubTask> GetSubTasksByEngineerId(string engineerId)
        {
            return set.Where(st => st.FK_EngineerID == engineerId).Include(s => s.Engineer).Include(st => st.Task).ThenInclude(t => t.Project);
        }

        public SubTask GetSubTaskIncludingTask(int subTaskId)
        {
            return set.Where(sub => sub.Id == subTaskId).Include(sub => sub.Task).FirstOrDefault();
        }

        public SubTask GetSubTaskIncludingProject(int subTaskId)
        {
            return set.Where(sub => sub.Id == subTaskId).Include(sub => sub.Task).ThenInclude(t => t.Project).FirstOrDefault();
        }

        //Get only SubTasks which is on hold or active
        public List<SubTask> GetEngineerSubTasks(string EngineerId)
        {
            return set.Where(sub => sub.FK_EngineerID == EngineerId)
                .Where(s => s.Status == Status.OnHold || s.Status == Status.Active)
                .Include(s => s.Engineer).Include(s => s.Task).ThenInclude(t => t.Project).ToList();
        }

        public List<SubTask> GetEngineerComletedSubTasks(string EngineerId)
        {
            return set.Where(sub => sub.FK_EngineerID == EngineerId)
                .Where(s => s.Status == Status.Completed)
                .Include(s => s.Engineer).ToList();
        }

        public IEnumerable<SubTask> GetEngineerCancelledSubTasks(string EngineerId)
        {
            return set.Where(sub => sub.FK_EngineerID == EngineerId)
                .Where(s => s.Status == Status.Cancelled)
                .Include(s => s.Engineer).Include(s => s.Task).ThenInclude(t => t.Project).ToList();
        }

        public SubTask GetSubTaskWithEngineer(int id)
        {
            return set.Where(s => s.Id == id).Include(s => s.Engineer).FirstOrDefault();
        }


        public SubTask GetSubTaskWithTeam(int id)
        {
            return set.Where(s => s.Id == id).Include(s => s.Task).ThenInclude(t => t.Team).FirstOrDefault();
        }

        //get All Subtasks not including cancelled
        public IEnumerable<SubTask> Get_Not_C_SubTasksFromTask(int taskId)
        {
            return set.Where(st => st.FK_TaskId == taskId)
                .Where(st => st.Status != Status.Cancelled).Include(s => s.Task).Include(s=>s.SubTaskSession).ToList();
        }

        //----------Not Working
        public List<SubTask> GetAllSubTasksWithTask(int taskId)
        {
            return set.Where(st => st.FK_TaskId == taskId).Include(s=>s.Comments).ToList();
        }


        public IEnumerable<SubTask> Archive(string EngineerId)
        {
            return set.Where(sub => sub.FK_EngineerID == EngineerId)
                .Where(s => s.Status == Status.Cancelled || s.Status == Status.Completed)
                .Include(s => s.Engineer).Include(s => s.Task).ThenInclude(t => t.Project).ToList();
        }
    }
}
