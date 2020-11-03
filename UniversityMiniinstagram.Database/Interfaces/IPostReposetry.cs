using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Database.Interfases
{
    public interface IPostReposetry
    {
        public ICollection<Post> GetAllPosts();
        public ICollection<Like> GetLikes(Guid postId);
        public ICollection<Comment> GetComments(Guid postId);
        public void AddPost(Post post);
        public void AddComment(Comment comment);
        public Comment GetComment(Guid commentId);
        public void AddLike(Like like);
        public void RemoveLike(Guid postId, string userId, DatabaseContext db = null);
        public ICollection<Post> GetUserPosts(string userId);
        public Post GetPost(Guid postId);
        public void RemoveComment(Comment comment);
        public void DeletePost(Post post);
        public void UpdatePost(Post post);
        public bool IsCommentReported(Guid commentId, string userId);
        public bool IsPostReported(Guid postId, string userId);
    }
}
