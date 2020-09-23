using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class PostServices : IPostServices
    {
        public PostServices(DatabaseContext context, ImageServices imageServices)
        {
            _context = context;
            _imageServices = imageServices;
        }
        
        DatabaseContext _context;
        ImageServices _imageServices;
        public async Task<Post> AddPost(PostViewModel vm, string rootPath, string userId)
        {
            var image = await _imageServices.Add(new ImageViewModel() { File = vm.File, Category = vm.Category }, rootPath);
            if(image != null)
            {
                Post newPost = new Post()
                {
                    Id = Guid.NewGuid(),
                    Description = vm.Description,
                    Image = image,
                    UploadDate = DateTime.Now,
                    UserId = userId
                };
                _context.Posts.Add(newPost);
                _context.SaveChanges();
                return newPost;
            }
            return null;
        }

        public Comment AddComment(CommentViewModel vm, string userId)
        {
            Comment newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Text = vm.Text,
                UserId = userId,
                PostId = vm.PostId,
            };
            _context.Comments.Add(newComment);
            _context.SaveChanges();
            return newComment;
        }

        public Like AddLike(Guid postId, string userId)
        {
            Like newLike = new Like()
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
            };
            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return newLike;
        }
    }
}
