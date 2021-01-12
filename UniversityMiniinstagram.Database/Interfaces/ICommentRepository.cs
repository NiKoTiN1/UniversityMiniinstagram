using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        public ICollection<Comment> GetAll(string postId);
        public bool IsCommentReported(string commentId, string userId);
    }
}
