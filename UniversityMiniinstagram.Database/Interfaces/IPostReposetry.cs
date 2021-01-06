using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IPostReposetry
    {
        public ICollection<Post> GetAllPosts();
        public ICollection<Like> GetLikes(Guid postId);
        public ICollection<Comment> GetComments(Guid postId);
        public Task AddPost(Post post);
        public Task AddComment(Comment comment);
        public Comment GetComment(Guid commentId);
        public Task AddLike(Like like);
        public Task RemoveLike(Guid postId, string userId, DatabaseContext db = null);
        public ICollection<Post> GetUserPosts(string userId);
        public Post GetPost(Guid postId);
        public Task RemoveComment(Comment comment);
        public Task DeletePost(Post post);
        public Task UpdatePost(Post post);
        public Task UpdateComment(Comment comment);
        public bool IsCommentReported(Guid commentId, string userId);
        public bool IsPostReported(Guid postId, string userId);
    }
}
