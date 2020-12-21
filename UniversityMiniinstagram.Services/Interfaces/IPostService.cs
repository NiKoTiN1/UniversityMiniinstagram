using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IPostService
    {
        public Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId);
        public Task<Comment> AddComment(SendCommentViewModel vm, string userId);
        public void DeletePost(Post post);
        public Like AddLike(Guid postId, string userId);
        public void RemoveLike(Guid postId, string userId, DatabaseContext db = null);
        public Task<ICollection<Post>> GetAllPosts();
        public ICollection<Post> GetUserPosts(string userId);
        public Task<Post> GetPost(Guid postId);
        public bool IsLiked(Guid postId, string userId);
        public Comment GetComment(Guid commentId);
        public bool HideComment(Guid commId);
        public Guid RemoveComment(Guid commentId);
        public Task<bool> IsDeleteRelated(ApplicationUser postHolder, string guestId);
        public Task<bool> IsReportRelated(string postHolderId, string guestId, Guid postId = new Guid(), Guid commentId = new Guid());
        public bool HidePost(Guid postId);
    }
}
