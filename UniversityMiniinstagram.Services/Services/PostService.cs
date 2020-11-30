using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public PostService(IImageService imageServices, IPostReposetry postReposetry, IAccountService accountService)
        {
            _imageServices = imageServices;
            _postReposetry = postReposetry;
            _accountService = accountService;
        }
        
        IImageService _imageServices;
        IPostReposetry _postReposetry;
        IAccountService _accountService;
        public async Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId)
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

        public async Task<Comment> AddComment(SendCommentViewModel vm, string userId)
        {
            var user = await _accountService.GetUser(userId);
            Comment newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Text = vm.Text,
                UserId = userId,
                PostId = vm.PostId,
                User = user
            };
            _postReposetry.AddComment(newComment);
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
            _postReposetry.AddLike(newLike);
            return newLike;
        }
        public bool IsLiked(Guid postId, string userId)
        {
            var likes = _postReposetry.GetLikes(postId);
            var isLiked = likes.Where(like => like.UserId == userId);
            return isLiked.Count() != 0;
        }
        public void RemoveLike(Guid postId, string userId, DatabaseContext db=null)
        {
            _postReposetry.RemoveLike(postId, userId, db);
        }

        public async Task<ICollection<Post>> GetAllPosts()
        {
            var posts = _postReposetry.GetAllPosts();
            foreach (var post in posts)
            {
                ICollection<Like> likes = _postReposetry.GetLikes(post.Id);
                ICollection<Comment> coments = _postReposetry.GetComments(post.Id);
                var timeZone = TimeZoneInfo.Local;
                foreach (var comment in coments)
                {
                    comment.User = await _accountService.GetUser(comment.UserId);
                }
                post.Likes = likes;
                post.Image = _imageServices.GetImage(post.ImageId);
                post.Comments = coments;
                post.User = await _accountService.GetUser(post.UserId);
            }
            return posts.ToList();
        }

        public Comment GetComment(Guid commentId)
        {
            var comment = _postReposetry.GetComment(commentId);
            return comment;
        }

        public void DeletePost(Post post)
        {
            for (int i = post.Comments.Count - 1; i >= 0; i--) 
            {
                _postReposetry.RemoveComment(post.Comments.ElementAt(i));
            }

            for (int i = post.Likes.Count - 1; i >= 0; i--)
            {
                var like = post.Likes.ElementAt(i);
                _postReposetry.RemoveLike(like.PostId, like.UserId);
            }
            _imageServices.RemoveImage(post.Image);
            var ppost = _postReposetry.GetPost(post.Id);
            if (ppost != null)
            {
                _postReposetry.DeletePost(ppost);
            }
        }

        public ICollection<Post> GetUserPosts(string userId)
        {
            var userPosts = _postReposetry.GetUserPosts(userId);
            foreach(var post in userPosts)
            {
                post.Image = _imageServices.GetImage(post.ImageId);
            }
            return userPosts;
        }

        public async Task<Post> GetPost(Guid postId)
        {
            var post = _postReposetry.GetPost(postId);
            if(post != null)
            {
                var image = _imageServices.GetImage(post.ImageId);
                if(image != null)
                {
                    post.Image = image;
                }
                var likes = _postReposetry.GetLikes(postId);
                var comments = _postReposetry.GetComments(postId);
                foreach(var comment in comments)
                {
                    comment.User = await _accountService.GetUser(comment.UserId);
                }
                var user = await _accountService.GetUser(post.UserId);
                post.User = user;
                post.Likes = likes;
                post.Comments = comments;
            }
            return post;
        }

        public Guid RemoveComment(Guid commentId)
        {
            var comment = _postReposetry.GetComment(commentId);
            var postId = comment.PostId;
            if(comment != null)
            {
                _postReposetry.RemoveComment(comment);
                return postId;
            }
            return new Guid();
        }
        public async Task<bool> isDeleteRelated(ApplicationUser postHolder, string guestId)
        {
            if(postHolder.Id == guestId)
            {
                return true;
            }
            var guest = await _accountService.GetUser(guestId);
            if(await _accountService.IsInRole(new IsInRoleViewModel() { user = guest, roleName = "Admin" }))
            {
                return true;
            }
            if (await _accountService.IsInRole(new IsInRoleViewModel() { user = guest, roleName = "Moderator" }))
            {
                if(!await _accountService.IsInRole(new IsInRoleViewModel() { user = guest, roleName = "Admin" }))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> isReportRelated(string postHolderId, string guestId, Guid postId=new Guid(), Guid commentId = new Guid())
        {
            if (postHolderId == guestId)
            {
                return false;
            }
            if(postId != new Guid())
            {
                if (_postReposetry.IsPostReported(postId, guestId))
                {
                    return false;
                }
            }
            if (commentId != new Guid())
            {
                if (_postReposetry.IsCommentReported(commentId, guestId))
                {
                    return false;
                }
            }
            var postHolderUser = await _accountService.GetUser(postHolderId);
            IsInRoleViewModel isInRolePostHolderVM = new IsInRoleViewModel()
            {
                user = postHolderUser,
                roleName = "Admin"
            };
            if(await _accountService.IsInRole(isInRolePostHolderVM))
            {
                return false;
            }
            return true;
        }

        public bool HidePost(Guid postId)
        {
            var post = _postReposetry.GetPost(postId);
            if(post == null)
            {
                return false;
            }
            post.IsShow = false;
            _postReposetry.UpdatePost(post);
            return true;
        }

        public bool HideComment(Guid commId)
        {
            var comment = _postReposetry.GetComment(commId);
            if (comment == null)
            {
                return false;
            }
            comment.IsShow = false;
            _postReposetry.UpdateComment(comment);
            return true;
        }
    }
}
