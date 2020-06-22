using ITI.CEI40.Monitor.Data.Repositories.Managers.Interfaces;
using ITI.CEI40.Monitor.Entities;
using ITI.CEI40.Monitor.Entities.Enums;
using Microsoft.EntityFrameworkCore;
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
            return set.Where(c => c.FK_TaskID == taskID).Include(c => c.Sender)
                .Include(c => c.File).ToList().OrderBy(c => c.commentTime);
        }

        public IEnumerable<Comment> GetCommentsForEngineer(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.FK_SubTaskId != null).ToList();
        }

        public IEnumerable<Comment> GetCommentsForTeamLeader(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel != CommentLevels.High).Include(c=>c.File).ToList();
        }

        public IEnumerable<Comment> GetCommentsForDepManager(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel != CommentLevels.low ).ToList();
        }

        public IEnumerable<Comment> GetCommentsForProjectManager(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel == CommentLevels.High).Include(c=>c.File).ToList();
        }

        public IEnumerable<Comment> GetHighCommentforTask(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel == CommentLevels.High).Include(c => c.File).ToList();
        }

        public IEnumerable<Comment> GetMedCommentforTask(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel == CommentLevels.Med).Include(c => c.File).ToList();
        }

        public IEnumerable<Comment> GetLowCommentforTask(int taskId)
        {
            return set.Where(c => c.FK_TaskID == taskId && c.commentLevel == CommentLevels.low).Include(s=>s.Sender).Include(c => c.File).ToList();
        }


    }
}