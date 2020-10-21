using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfases;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class PostService : IPostService
    {
        public PostService(DatabaseContext context, IImageService imageServices, IPostReposetry postReposetry, IAccountReposetry accountReposetry)
        {
            _context = context;
            _imageServices = imageServices;
            _postReposetry = postReposetry;
            _accountReposetry = accountReposetry;
        }
        
        DatabaseContext _context;
        IImageService _imageServices;
        IPostReposetry _postReposetry;
        IAccountReposetry _accountReposetry;
        public async Task<Post> AddPost(PostViewModel vm, string rootPath, string userId)
        {
            var image = await _imageServices.Add(new ImageViewModel() { File = vm.File }, rootPath);
            if(image != null)
            {
                Post newPost = new Post()
                {
                    Id = Guid.NewGuid(),
                    Description = vm.Description,
                    Image = image,
                    UploadDate = DateTime.Now,
                    UserId = userId,
                    category = vm.category
                };
                _postReposetry.AddPost(newPost);
                return newPost;
            }
            return null;
        }

        public async Task<string> AddComment(CommentViewModel vm, string userId)
        {
            var user = await _accountReposetry.GetUser(userId);
            Comment newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Text = vm.Text,
                UserId = userId,
                PostId = vm.PostId,
            };
            _postReposetry.AddComment(newComment);
            string block = "<div class=\"media chat-item\">" +
                "<div class=\"media-body\">" +
                    "<div class=\"chat-item-title\">" +
                        "<span class=\"font-weight-bold\" data-filter-by=\"text\">" + user.UserName + "</span>" +
                    "</div>" +
                    "<div class=\"chat-item-body DIV-filter-by-text\" data-filter-by=\"text\">" +
                        "<p>" + vm.Text + "</p>" +
                    "</div>" +
                "</div>" +
                "</div>";
            return block;
        }

        public Like AddLike(Guid postId, string userId)
        {
            Like newLike = new Like()
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
            };
            _postReposetry.AddLike(newLike);
            return newLike;
        }
        public void RemoveLike(Guid postId, string userId)
        {
            _postReposetry.RemoveLike(postId, userId);
        }

        public async Task<ICollection<Post>> GetAllPosts()
        {
            var posts = _postReposetry.GetAllPosts();
            foreach (var post in posts)
            {
                ICollection<Like> likes = _postReposetry.GetLikes(post.Id);
                ICollection<Comment> coments = _postReposetry.GetComments(post.Id);
                foreach (var comment in coments)
                {
                    comment.User = await _accountReposetry.GetUser(comment.UserId);
                }
                post.Likes = likes;
                post.Image = _postReposetry.GetImage(post.ImageId);
                post.Comments = coments;
            }
            return posts.ToList();
        }

        public ICollection<Post> GetUserPosts(string userId)
        {
            var userPosts = _postReposetry.GetUserPosts(userId);
            foreach(var post in userPosts)
            {
                post.Image = _postReposetry.GetImage(post.ImageId);
            }
            return userPosts;
        }
    }
}
