using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class PostService : IPostService
    {
        public PostService(IImageService imageServices, IPostReposetry postReposetry, IAccountService accountService)
        {
            this.ImageServices = imageServices;
            this.PostReposetry = postReposetry;
            this.AccountService = accountService;
        }

        private readonly IImageService ImageServices;
        private readonly IPostReposetry PostReposetry;
        private readonly IAccountService AccountService;
        public async Task<Post> AddPost(CreatePostViewModel vm, string rootPath, string userId)
        {
            Image image = await this.ImageServices.Add(new ImageViewModel() { File = vm.File }, rootPath);
            if (image != null)
            {
                var newPost = new Post()
                {
                    Id = Guid.NewGuid(),
                    Description = vm.Description,
                    Image = image,
                    UploadDate = DateTime.Now,
                    UserId = userId,
                    CategoryPost = vm.CategoryPost
                };
                this.PostReposetry.AddPost(newPost);
                return newPost;
            }
            return null;
        }

        public async Task<Comment> AddComment(SendCommentViewModel vm, string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            var newComment = new Comment()
            {
                Id = Guid.NewGuid(),
                Text = vm.Text,
                UserId = userId,
                PostId = vm.PostId,
                User = user
            };
            this.PostReposetry.AddComment(newComment);
            return newComment;
        }

        public Like AddLike(Guid postId, string userId)
        {
            var newLike = new Like()
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                UserId = userId,
            };
            this.PostReposetry.AddLike(newLike);
            return newLike;
        }
        public bool IsLiked(Guid postId, string userId)
        {
            ICollection<Like> likes = this.PostReposetry.GetLikes(postId);
            IEnumerable<Like> isLiked = likes.Where(like => like.UserId == userId);
            return isLiked.Count() != 0;
        }
        public void RemoveLike(Guid postId, string userId, DatabaseContext db = null)
        {
            this.PostReposetry.RemoveLike(postId, userId, db);
        }

        public async Task<ICollection<Post>> GetAllPosts()
        {
            ICollection<Post> posts = this.PostReposetry.GetAllPosts();
            foreach (Post post in posts)
            {
                ICollection<Like> likes = this.PostReposetry.GetLikes(post.Id);
                ICollection<Comment> coments = this.PostReposetry.GetComments(post.Id);
                foreach (Comment comment in coments)
                {
                    comment.User = await this.AccountService.GetUser(comment.UserId);
                }
                post.Likes = likes;
                post.Image = this.ImageServices.GetImage(post.ImageId);
                post.Comments = coments;
                post.User = await this.AccountService.GetUser(post.UserId);
            }
            return posts.ToList();
        }

        public Comment GetComment(Guid commentId)
        {
            Comment comment = this.PostReposetry.GetComment(commentId);
            return comment;
        }

        public void DeletePost(Post post)
        {
            for (var i = post.Comments.Count - 1; i >= 0; i--)
            {
                this.PostReposetry.RemoveComment(post.Comments.ElementAt(i));
            }

            for (var i = post.Likes.Count - 1; i >= 0; i--)
            {
                Like like = post.Likes.ElementAt(i);
                this.PostReposetry.RemoveLike(like.PostId, like.UserId);
            }
            this.ImageServices.RemoveImage(post.Image);
            Post ppost = this.PostReposetry.GetPost(post.Id);
            if (ppost != null)
            {
                this.PostReposetry.DeletePost(ppost);
            }
        }

        public ICollection<Post> GetUserPosts(string userId)
        {
            ICollection<Post> userPosts = this.PostReposetry.GetUserPosts(userId);
            foreach (Post post in userPosts)
            {
                post.Image = this.ImageServices.GetImage(post.ImageId);
            }
            return userPosts;
        }

        public async Task<Post> GetPost(Guid postId)
        {
            Post post = this.PostReposetry.GetPost(postId);
            if (post != null)
            {
                Image image = this.ImageServices.GetImage(post.ImageId);
                if (image != null)
                {
                    post.Image = image;
                }
                ICollection<Like> likes = this.PostReposetry.GetLikes(postId);
                ICollection<Comment> comments = this.PostReposetry.GetComments(postId);
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

        public Guid RemoveComment(Guid commentId)
        {
            Comment comment = this.PostReposetry.GetComment(commentId);
            Guid postId = comment.PostId;
            if (comment != null)
            {
                this.PostReposetry.RemoveComment(comment);
                return postId;
            }
            return new Guid();
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
        public async Task<bool> IsReportRelated(string postHolderId, string guestId, Guid postId = new Guid(), Guid commentId = new Guid())
        {
            if (postHolderId == guestId)
            {
                return false;
            }
            if (postId != new Guid())
            {
                if (this.PostReposetry.IsPostReported(postId, guestId))
                {
                    return false;
                }
            }
            if (commentId != new Guid())
            {
                if (this.PostReposetry.IsCommentReported(commentId, guestId))
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

        public bool HidePost(Guid postId)
        {
            Post post = this.PostReposetry.GetPost(postId);
            if (post == null)
            {
                return false;
            }
            post.IsShow = false;
            this.PostReposetry.UpdatePost(post);
            return true;
        }

        public bool HideComment(Guid commId)
        {
            Comment comment = this.PostReposetry.GetComment(commId);
            if (comment == null)
            {
                return false;
            }
            comment.IsShow = false;
            this.PostReposetry.UpdateComment(comment);
            return true;
        }
    }
}
