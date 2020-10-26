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
        public Like AddLike(Guid postId, string userId);
        public void RemoveLike(Guid postId, string userId);
        public Task<ICollection<Post>> GetAllPosts();
        public ICollection<Post> GetUserPosts(string userId);
        public Task<Post> GetPost(Guid postId);
        public Guid RemoveComment(Guid commentId);
        public Task<bool> isDeleteRelated(ApplicationUser postHolder, string guestId);
        public bool isReportRelated(string postHolderId, string guestId);
    }
}
