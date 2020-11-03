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
        public Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId);
        public Task<Comment> AddComment(SendCommentViewModel vm, string userId);
        public void DeletePost(Post post);
        public Like AddLike(Guid postId, string userId);
        public void RemoveLike(Guid postId, string userId, DatabaseContext db = null);
        public Task<ICollection<Post>> GetAllPosts();
        public ICollection<Post> GetUserPosts(string userId);
        public Task<Post> GetPost(Guid postId);
        public Guid RemoveComment(Guid commentId);
        public Task<bool> isDeleteRelated(ApplicationUser postHolder, string guestId);
        public Task<bool> isReportRelated(string postHolderId, string guestId, Guid postId = new Guid(), Guid commentId = new Guid());
        public bool HidePost(Guid postId);
    }
}
