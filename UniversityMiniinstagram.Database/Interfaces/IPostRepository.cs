using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        public bool IsPostReported(string postId, string userId);
    }
}
