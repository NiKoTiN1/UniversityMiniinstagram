using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfases;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class PostReposetry : IPostReposetry
    {
        public PostReposetry(DatabaseContext context)
        {
            _context = context;
        }

        DatabaseContext _context;
        public ICollection<Post> GetAllPosts()
        {
            ICollection<Post> posts = _context.Posts.OrderBy(post => post.UploadDate).ToList();
            return posts;
        }

        public ICollection<Like> GetLikes(Guid postId)
        {
            ICollection<Like> likes = _context.Likes.Where(like => like.PostId == postId).ToList();
            return likes;
        }

        public ICollection<Comment> GetComments(Guid postId)
        {
            ICollection<Comment> comments = _context.Comments.Where(comment => comment.PostId == postId).ToList();
            return comments;
        }
        public Comment GetComment(Guid commentId)
        {
            var comment = _context.Comments.FirstOrDefault(comment => comment.Id == commentId);
            return comment;
        }

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void AddLike(Like like)
        {
            _context.Likes.Add(like);
            _context.SaveChanges();
        }

        public void RemoveLike(Guid postId, string userId)
        {
            var like = _context.Likes.FirstOrDefault(li => li.PostId == postId && li.UserId == userId);
            _context.Likes.Remove(like);
            _context.SaveChanges();
        }

        public ICollection<Post> GetUserPosts(string userId)
        {
            var userPosts = _context.Posts.Where(post => post.UserId == userId).OrderBy(post => post.UploadDate).ToList();
            return userPosts;
        }

        public Post GetPost(Guid postId)
        {
            var post = _context.Posts.FirstOrDefault(post => post.Id == postId);
            return post;
        }

        public void RemoveComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
