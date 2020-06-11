using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Data.Repositories.Managers
{
    public class CommentManager : Reposiotry<ApplicationDbContext, Comment>, ICommentManager
    {
        public CommentManager(ApplicationDbContext context) : base(context)
        {

        }

        public IEnumerable<Comment> GetCommentsForTask(int taskID)
        {
            return set.Where(c => c.fk_TaskId == taskID).ToList();
        }

    }
}