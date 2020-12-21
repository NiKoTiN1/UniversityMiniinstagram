using System;
using System.Collections.Generic;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class PostReposetry : IPostReposetry
    {
        public PostReposetry(DatabaseContext context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;
        public ICollection<Post> GetAllPosts()
        {
            ICollection<Post> posts = this.Context.Posts.OrderBy(post => post.UploadDate).ToList();
            return posts;
        }

        public ICollection<Like> GetLikes(Guid postId)
        {
            ICollection<Like> likes = this.Context.Likes.Where(like => like.PostId == postId).ToList();
            return likes;
        }

        public ICollection<Comment> GetComments(Guid postId)
        {
            ICollection<Comment> comments = this.Context.Comments.Where(comment => comment.PostId == postId).OrderBy(comment => comment.Date).ToList();
            return comments;
        }
        public Comment GetComment(Guid commentId)
        {
            Comment comment = this.Context.Comments.FirstOrDefault(comment => comment.Id == commentId);
            return comment;
        }

        public void AddPost(Post post)
        {
            post.IsShow = true;
            this.Context.Posts.Add(post);
            this.Context.SaveChanges();
        }

        public void AddComment(Comment comment)
        {
            comment.IsShow = true;
            comment.Date = DateTime.UtcNow;
            this.Context.Comments.Add(comment);
            this.Context.SaveChanges();
        }

        public void AddLike(Like like)
        {
            this.Context.Likes.Add(like);
            this.Context.SaveChanges();
        }

        public void RemoveLike(Guid postId, string userId, DatabaseContext db = null)
        {
            if (db != null)
            {
                Like likee = db.Likes.FirstOrDefault(li => li.PostId == postId && li.UserId == userId);
                db.Likes.Remove(likee);
                db.SaveChanges();
                return;
            }
            Like like = this.Context.Likes.FirstOrDefault(li => li.PostId == postId && li.UserId == userId);
            this.Context.Likes.Remove(like);
            this.Context.SaveChanges();
        }

        public ICollection<Post> GetUserPosts(string userId)
        {
            var userPosts = this.Context.Posts.Where(post => post.UserId == userId).OrderBy(post => post.UploadDate).ToList();
            return userPosts;
        }

        public Post GetPost(Guid postId)
        {
            Post post = this.Context.Posts.FirstOrDefault(post => post.Id == postId);
            return post;
        }

        public void RemoveComment(Comment comment)
        {
            IQueryable<Report> reports = this.Context.Reports.Where(report => report.CommentId == comment.Id);
            foreach (Report report in reports)
            {
                this.Context.Reports.Remove(report);
            }
            this.Context.Comments.Remove(comment);
            this.Context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            IQueryable<Report> reports = this.Context.Reports.Where(report => report.PostId == post.Id);
            foreach (Report report in reports)
            {
                this.Context.Reports.Remove(report);
            }
            this.Context.Posts.Remove(post);
            this.Context.SaveChanges();
            return;
        }

        public void UpdatePost(Post post)
        {
            this.Context.Posts.Update(post);
            this.Context.SaveChanges();
        }

        public void UpdateComment(Comment comment)
        {
            this.Context.Comments.Update(comment);
            this.Context.SaveChanges();
        }

        public bool IsCommentReported(Guid commentId, string userId)
        {
            var result = this.Context.Reports.Where(report => report.CommentId == commentId && report.UserId == userId).ToList();
            return result.Count != 0;
        }

        public bool IsPostReported(Guid postId, string userId)
        {
            var result = this.Context.Reports.Where(report => report.PostId == postId && report.UserId == userId).ToList();
            return result.Count != 0;
        }
    }
}
