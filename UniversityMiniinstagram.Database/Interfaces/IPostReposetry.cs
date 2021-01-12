using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IPostReposetry : IBaseReposetry<Post>
    {
        public ICollection<Post> GetUserPosts(string userId);
        public bool IsPostReported(string postId, string userId);
    }
}
