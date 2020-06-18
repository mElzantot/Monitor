using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces
{
    public interface ICommentManager : IRepository<Comment>
    {
        IEnumerable<Comment> GetCommentsForTask(int taskID);

        IEnumerable<Comment> GetCommentsForEngineer(int taskId);

        IEnumerable<Comment> GetCommentsForTeamLeader(int taskId);

        IEnumerable<Comment> GetCommentsForDepManager(int taskId);

        IEnumerable<Comment> GetCommentsForProjectManager(int taskId);

        IEnumerable<Comment> GetLowCommentforTask(int taskId);
        IEnumerable<Comment> GetMedCommentforTask(int taskId);
        IEnumerable<Comment> GetHighCommentforTask(int taskId);



    }
}