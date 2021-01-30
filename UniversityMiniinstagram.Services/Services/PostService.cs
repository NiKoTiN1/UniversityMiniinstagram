using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Constants;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;

namespace UniversityMiniinstagram.Services
{
    public class PostService : IPostService
    {
        public PostService(IImageService imageServices,
            IPostRepository postReposetry,
            IAccountService accountService,
            ICommentRepository commentReposetry,
            ILikeRepository likeReposetry,
            IAdminRepository adminRepository,
            ICommentReportReposetory commentReportReposetory)
        {
            this.ImageServices = imageServices;
            this.PostReposetry = postReposetry;
            this.AccountService = accountService;
            this.commentReposetry = commentReposetry;
            this.likeReposetry = likeReposetry;
            this.adminRepository = adminRepository;
            this.commentReportReposetory = commentReportReposetory;
        }

        private readonly IImageService ImageServices;
        private readonly IPostRepository PostReposetry;
        private readonly IAccountService AccountService;
        private readonly ICommentRepository commentReposetry;
        private readonly ILikeRepository likeReposetry;
        private readonly IAdminRepository adminRepository;
        private readonly ICommentReportReposetory commentReportReposetory;

        public async Task<Post> AddPost(IFormFile file, string rootPath, string userId, string description, Category categoryPost)
        {
            Image image = await this.ImageServices.Add(file, rootPath);
            if (image == null)
            {
                return null;
            }
            var newPost = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Image = image,
                UploadDate = DateTime.Now,
                UserId = userId,
                CategoryPost = categoryPost,
                IsShow = true
            };
            await this.PostReposetry.Add(newPost);
            return newPost;
        }

        public async Task<Comment> AddComment(string postId, string text, string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            if (user == null)
            {
                return null;
            }
            var newComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Text = text,
                UserId = userId,
                PostId = postId,
                User = user,
                IsShow = true,
                Date = DateTime.UtcNow
            };
            await this.commentReposetry.Add(newComment);
            return newComment;
        }
        public async Task<Like> AddLike(string postId, string userId)
        {
            var newLike = new Like()
            {
                Id = Guid.NewGuid().ToString(),
                PostId = postId,
                UserId = userId,
            };
            await this.likeReposetry.Add(newLike);
            return newLike;
        }
        public async Task<bool> IsLiked(string postId, string userId)
        {
            return (await this.likeReposetry.Get(like => like.PostId == postId && like.UserId == userId)).Any();
        }
        public async Task RemoveLike(string postId, string userId)
        {
            Like like = (await this.likeReposetry.Get(li => li.PostId == postId && li.UserId == userId)).SingleOrDefault();
            await this.likeReposetry.Remove(like);
        }
        public async Task<List<Post>> GetAllPosts(string userId)
        {
            return (await this.PostReposetry.Get(post => true, new string[] { "Comments.User", "Likes", "User" })).OrderBy(post => post.UploadDate).ToList();
        }

        public async Task DeletePost(string postId, Post post = null)
        {
            if (post == null)
            {
                post = await GetPost(postId);
            }
            this.ImageServices.RemoveImage(post.Image);
            if (post != null)
            {
                await this.PostReposetry.Remove(post);
            }
        }

        public async Task<ApplicationUser> GetUserPosts(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            user.Posts = (await this.PostReposetry.Get(post => post.UserId == userId)).OrderBy(post => post.UploadDate).ToList();
            return user;
        }

        public async Task<Post> GetPost(string postId)
        {
            Post post = (await this.PostReposetry.Get(post => post.Id == postId, new string[] { "Comments.User", "Likes" })).SingleOrDefault();
            post.Comments = post.Comments.OrderBy(comment => comment.Date).ToList();
            return post;
        }
        public async Task<Post> GetProfilePost(string postId)
        {
            return await GetPost(postId);
        }

        public async Task<string> RemoveComment(string commentId)
        {
            Comment comment = (await this.commentReposetry.Get(comment => comment.Id == commentId)).SingleOrDefault();
            if (comment == null)
            {
                return null;
            }
            await this.commentReposetry.Remove(comment);
            return comment.PostId;
        }
        public async Task<bool> IsDeleteAllowed(ApplicationUser postHolder, string guestId)
        {
            if (postHolder.Id == guestId)
            {
                return true;
            }
            return await this.AccountService.IsInRole(guestId, Enum.GetName(typeof(Roles), Roles.Admin))
                ? true
                : await this.AccountService.IsInRole(guestId, Enum.GetName(typeof(Roles), Roles.Moderator))
                ? !await this.AccountService.IsInRole(guestId, Enum.GetName(typeof(Roles), Roles.Admin))
                : false;
        }
        public async Task<bool> IsReportAllowed(string postHolderId, string guestId, string postId = null, string commentId = null)
        {
            if (postHolderId == guestId)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(postId))
            {
                if ((await this.adminRepository.Get(report => report.PostId == postId && report.UserId == guestId)).Any())
                {
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(commentId))
            {
                if ((await this.commentReportReposetory.Get(report => report.CommentId == commentId && report.UserId == guestId)).Any())
                {
                    return false;
                }
            }
            return !await this.AccountService.IsInRole(postHolderId, Enum.GetName(typeof(Roles), Roles.Admin));
        }

        public async Task<bool> HidePost(string postId)
        {
            Post post = (await this.PostReposetry.Get(post => post.Id == postId)).SingleOrDefault();
            if (post == null)
            {
                return false;
            }
            post.IsShow = false;
            await this.PostReposetry.Update(post);
            return true;
        }

        public async Task<bool> HideComment(string commId)
        {
            Comment comment = (await this.commentReposetry.Get(comment => comment.Id == commId)).SingleOrDefault();
            if (comment == null)
            {
                return false;
            }
            comment.IsShow = false;
            await this.commentReposetry.Update(comment);
            return true;
        }
    }
}
