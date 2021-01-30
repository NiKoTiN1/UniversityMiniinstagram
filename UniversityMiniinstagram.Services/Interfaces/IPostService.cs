using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post> AddPost(IFormFile file, string rootPath, string userId, string description, Category categoryPost);
        public Task<Comment> AddComment(string postId, string text, string userId);
        public Task DeletePost(string postId, Post post = null);
        public Task<Like> AddLike(string postId, string userId);
        public Task RemoveLike(string postId, string userId);
        public Task<List<Post>> GetAllPosts(string userId);
        public Task<ApplicationUser> GetUserPosts(string userId);
        public Task<Post> GetPost(string postId);
        public Task<Post> GetProfilePost(string postId);
        public Task<bool> IsLiked(string postId, string userId);
        public Task<bool> HideComment(string commId);
        public Task<string> RemoveComment(string commentId);
        public Task<bool> IsDeleteAllowed(ApplicationUser postHolder, string guestId);
        public Task<bool> IsReportAllowed(string postHolderId, string guestId, string postId = null, string commentId = null);
        public Task<bool> HidePost(string postId);
    }
}
