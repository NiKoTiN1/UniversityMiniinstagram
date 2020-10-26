﻿using System;
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
        public void RemoveLike(Guid postId, string userId);
        public ICollection<Post> GetUserPosts(string userId);
        public Post GetPost(Guid postId);
        public void RemoveComment(Comment comment);
    }
}