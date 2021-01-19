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
        public async Task<bool> IsLiked(string postId, string userId)
        {
            return (await this.likeReposetry.Get(like => like.PostId == postId && like.UserId == userId)).Any();
        }
        public async Task RemoveLike(string postId, string userId)
        {
            Like like = (await this.likeReposetry.Get(li => li.PostId == postId && li.UserId == userId)).SingleOrDefault();
            await this.likeReposetry.Remove(like);
        }

        public async Task<List<PostsViewModel>> GetAllPosts(string userId)
        {
            IEnumerable<Post> posts = (await this.PostReposetry.Get(post => true, new string[] { "Comments.User", "Likes", "User" })).OrderBy(post => post.UploadDate);
            var postsVM = new List<PostsViewModel>();
            foreach (Post post in posts)
            {
                var postVM = new PostsViewModel()
                {
                    Post = post,
                    IsDeleteAllowed = await IsDeleteAllowed(post.User, userId),
                    IsReportAllowed = await IsReportAllowed(post.UserId, userId, post.Id),
                    CommentVM = new List<CommentViewModel>()
                };
                foreach (Comment comment in post.Comments.OrderBy(comment => comment.Date))
                {
                    postVM.CommentVM.Add(new CommentViewModel()
                    {
                        Comment = comment,
                        IsDeleteAllowed = await IsDeleteAllowed(comment.User, userId),
                        IsReportAllowed = await IsReportAllowed(comment.UserId, userId, comment.Id),
                        ShowReportColor = false
                    });
                }
                postsVM.Add(postVM);
            }
            return postsVM;
        }

        public async Task DeletePost(Post post)
        {
            this.ImageServices.RemoveImage(post.Image);
            if (post != null)
            {
                await this.PostReposetry.Remove(post);
            }
        }

        public async Task<ICollection<Post>> GetUserPosts(string userId)
        {
            IEnumerable<Post> userPosts = (await this.PostReposetry.Get(post => post.UserId == userId)).OrderBy(post => post.UploadDate);
            return userPosts.ToList();
        }

        public async Task<Post> GetPost(string postId)
        {
            Post post = (await this.PostReposetry.Get(post => post.Id == postId, new string[] { "Comments.User", "Likes" })).SingleOrDefault();
            post.Comments = post.Comments.OrderBy(comment => comment.Date).ToList();
            return post;
        }

        public async Task<string> RemoveComment(string commentId)
        {
            Comment comment = (await this.commentReposetry.Get(comment => comment.Id == commentId)).SingleOrDefault();
            var postId = comment.PostId;
            if (comment != null)
            {
                await this.commentReposetry.Remove(comment);
                return postId;
            }
            return null;
        }
        public async Task<bool> IsDeleteAllowed(ApplicationUser postHolder, string guestId)
        {
            if (postHolder.Id == guestId)
            {
                return true;
            }
            ApplicationUser guest = await this.AccountService.GetUser(guestId);
            return await this.AccountService.IsInRole(guest, "Admin")
                ? true
                : await this.AccountService.IsInRole(guest, "Moderator")
                ? !await this.AccountService.IsInRole(guest, "Admin")
                : false;
        }
        public async Task<bool> IsReportAllowed(string postHolderId, string guestId, string postId = null, string commentId = null)
        {
            if (postHolderId == guestId)
            {
                return false;
            }
            if (string.IsNullOrEmpty(postId))
            {
                if ((await this.adminRepository.Get(report => report.PostId == postId && report.UserId == guestId)).Any())
                {
                    return false;
                }
            }
            if (string.IsNullOrEmpty(commentId))
            {
                if ((await this.commentReportReposetory.Get(report => report.CommentId == commentId && report.UserId == guestId)).Any())
                {
                    return false;
                }
            }
            ApplicationUser postHolderUser = await this.AccountService.GetUser(postHolderId);
            return !await this.AccountService.IsInRole(postHolderUser, "Admin");
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
