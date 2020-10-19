using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post> AddPost(PostViewModel vm, string rootPath, string userId);
        public Task<string> AddComment(CommentViewModel vm, string userId);
        public Like AddLike(Guid postId, string userId);
        public void RemoveLike(Guid postId, string userId);
        public Task<ICollection<Post>> GetAllPosts(string userId);
    }
}
