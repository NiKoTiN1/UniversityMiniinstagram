using System.Collections.Generic;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DatabaseContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        private readonly DatabaseContext dbContext;

        public ICollection<Comment> GetAll(string postId)
        {
            ICollection<Comment> comments = this.dbContext.Comments.Where(comment => comment.PostId == postId).OrderBy(comment => comment.Date).ToList();
            return comments;
        }

        public bool IsCommentReported(string commentId, string userId)
        {
            var result = this.dbContext.Reports.Where(report => report.CommentId == commentId && report.UserId == userId).ToList();
            return result.Count != 0;
        }
    }
}
