using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId);
        public Task<Comment> AddComment(SendCommentViewModel vm, string userId);
        public Task DeletePost(Post post);
        public Task<Like> AddLike(string postId, string userId);
        public Task RemoveLike(string postId, string userId);
        public Task<ICollection<Post>> GetAllPosts();
        public Task<ICollection<Post>> GetUserPosts(string userId);
        public Task<Post> GetPost(string postId);
        public bool IsLiked(string postId, string userId);
        public Task<Comment> GetComment(string commentId);
        public Task<bool> HideComment(string commId);
        public Task<string> RemoveComment(string commentId);
        public Task<bool> IsDeleteRelated(ApplicationUser postHolder, string guestId);
        public Task<bool> IsReportRelated(string postHolderId, string guestId, string postId = null, string commentId = null);
        public Task<bool> HidePost(string postId);
    }
}
