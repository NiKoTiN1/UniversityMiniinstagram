using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services
{
    public class PostService : IPostService
    {
        public PostService(IImageService imageServices,
            IPostRepository postReposetry,
            IAccountService accountService,
            ICommentRepository commentReposetry,
            ILikeRepository likeReposetry)
        {
            this.ImageServices = imageServices;
            this.PostReposetry = postReposetry;
            this.AccountService = accountService;
            this.commentReposetry = commentReposetry;
            this.likeReposetry = likeReposetry;
        }

        private readonly IImageService ImageServices;
        private readonly IPostRepository PostReposetry;
        private readonly IAccountService AccountService;
        private readonly ICommentRepository commentReposetry;
        private readonly ILikeRepository likeReposetry;

        public async Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId)
        {
            Image image = await this.ImageServices.Add(new ImageViewModel() { File = vm.File }, rootPath);
            if (image != null)
            {
                var newPost = new Post()
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = vm.Description,
                    Image = image,
                    UploadDate = DateTime.Now,
                    UserId = userId,
                    CategoryPost = vm.CategoryPost,
                    IsShow = true
                };
                await this.PostReposetry.Add(newPost);
                return newPost;
            }
            return null;
        }

        public async Task<Comment> AddComment(SendCommentViewModel vm, string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            var newComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Text = vm.Text,
                UserId = userId,
                PostId = vm.PostId,
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
        public bool IsLiked(string postId, string userId)
        {
            ICollection<Like> likes = this.likeReposetry.Get(postId);
            IEnumerable<Like> isLiked = likes.Where(like => like.UserId == userId);
            return isLiked.Any();
        }
        public async Task RemoveLike(string postId, string userId)
        {
            await this.likeReposetry.Remove(postId, userId);
        }

        public async Task<ICollection<Post>> GetAllPosts()
        {
            ICollection<Post> posts = await this.PostReposetry.GetAll();
            foreach (Post post in posts)
            {
                ICollection<Like> likes = this.likeReposetry.Get(post.Id);
                ICollection<Comment> coments = this.commentReposetry.GetAll(post.Id);
                foreach (Comment comment in coments)
                {
                    comment.User = await this.AccountService.GetUser(comment.UserId);
                }
                post.Likes = likes;
                post.Image = await this.ImageServices.GetImage(post.ImageId);
                post.Comments = coments;
                post.User = await this.AccountService.GetUser(post.UserId);
            }
            return posts.ToList();
        }

        public async Task<Comment> GetComment(string commentId)
        {
            return await this.commentReposetry.Get(commentId);
        }

        public async Task DeletePost(Post post)
        {
            await this.ImageServices.RemoveImage(post.Image);
            Post ppost = await this.PostReposetry.Get(post.Id);
            if (ppost != null)
            {
                await this.PostReposetry.Remove(ppost);
            }
        }

        public async Task<ICollection<Post>> GetUserPosts(string userId)
        {
            ICollection<Post> userPosts = this.PostReposetry.GetUserPosts(userId);
            foreach (Post post in userPosts)
            {
                post.Image = await this.ImageServices.GetImage(post.ImageId);
            }
            return userPosts;
        }

        public async Task<Post> GetPost(string postId)
        {
            Post post = await this.PostReposetry.Get(postId);
            if (post != null)
            {
                Image image = await this.ImageServices.GetImage(post.ImageId);
                if (image != null)
                {
                    post.Image = image;
                }
                ICollection<Like> likes = this.likeReposetry.Get(postId);
                ICollection<Comment> comments = this.commentReposetry.GetAll(postId);
                foreach (Comment comment in comments)
                {
                    comment.User = await this.AccountService.GetUser(comment.UserId);
                }
                ApplicationUser user = await this.AccountService.GetUser(post.UserId);
                post.User = user;
                post.Likes = likes;
                post.Comments = comments;
            }
            return post;
        }

        public async Task<string> RemoveComment(string commentId)
        {
            Comment comment = await this.commentReposetry.Get(commentId);
            var postId = comment.PostId;
            if (comment != null)
            {
                await this.commentReposetry.Remove(comment);
                return postId;
            }
            return null;
        }
        public async Task<bool> IsDeleteRelated(ApplicationUser postHolder, string guestId)
        {
            if (postHolder.Id == guestId)
            {
                return true;
            }
            ApplicationUser guest = await this.AccountService.GetUser(guestId);
            return await this.AccountService.IsInRole(new IsInRoleViewModel() { User = guest, RoleName = "Admin" })
                ? true
                : await this.AccountService.IsInRole(new IsInRoleViewModel() { User = guest, RoleName = "Moderator" })
                ? !await this.AccountService.IsInRole(new IsInRoleViewModel() { User = guest, RoleName = "Admin" })
                : false;
        }
        public async Task<bool> IsReportRelated(string postHolderId, string guestId, string postId = null, string commentId = null)
        {
            if (postHolderId == guestId)
            {
                return false;
            }
            if (postId != null)
            {
                if (this.PostReposetry.IsPostReported(postId, guestId))
                {
                    return false;
                }
            }
            if (commentId != null)
            {
                if (this.commentReposetry.IsCommentReported(commentId, guestId))
                {
                    return false;
                }
            }
            ApplicationUser postHolderUser = await this.AccountService.GetUser(postHolderId);
            var isInRolePostHolderVM = new IsInRoleViewModel()
            {
                User = postHolderUser,
                RoleName = "Admin"
            };
            return !await this.AccountService.IsInRole(isInRolePostHolderVM);
        }

        public async Task<bool> HidePost(string postId)
        {
            Post post = await this.PostReposetry.Get(postId);
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
            Comment comment = await this.commentReposetry.Get(commId);
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
