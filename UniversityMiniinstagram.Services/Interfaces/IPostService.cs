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
        public Task<List<PostsViewModel>> GetAllPosts(string userId);
        public Task<ICollection<Post>> GetUserPosts(string userId);
        public Task<Post> GetPost(string postId);
        public Task<bool> IsLiked(string postId, string userId);
        public Task<bool> HideComment(string commId);
        public Task<string> RemoveComment(string commentId);
        public Task<bool> IsDeleteAllowed(ApplicationUser postHolder, string guestId);
        public Task<bool> IsReportAllowed(string postHolderId, string guestId, string postId = null, string commentId = null);
        public Task<bool> HidePost(string postId);
    }
}
