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
    public class AdminService : IAdminService
    {
        public AdminService(IAdminRepository adminReposetry,
            IPostService postService,
            IAccountService accountService,
            ICommentReportReposetory commentReportReposetory)
        {
            this.AdminReposetry = adminReposetry;
            this.PostService = postService;
            this.AccountService = accountService;
            this.commentReportReposetory = commentReportReposetory;
        }

        private readonly IAdminRepository AdminReposetry;
        private readonly IPostService PostService;
        private readonly IAccountService AccountService;
        private readonly ICommentReportReposetory commentReportReposetory;

        public async Task ReportComment(string userId, string commentId)
        {
            await this.commentReportReposetory.Add(new CommentReport() { Id = Guid.NewGuid().ToString(), CommentId = commentId, UserId = userId, Date = DateTime.UtcNow });
        }

        public async Task ReportPost(string postId, string userId)
        {
            await this.AdminReposetry.Add(new PostReport() { Id = Guid.NewGuid().ToString(), PostId = postId, UserId = userId, Date = DateTime.UtcNow });
        }

        public async Task<ICollection<PostReport>> GetPostReports(string userId)
        {
            return (await this.AdminReposetry.Get(report => true, new string[] { "Post.Comments.User", "Post.Likes" })).OrderBy(report => report.Date).ToList();
        }

        public async Task<List<CommentReport>> GetCommentReports(string userId)
        {
            var isCurUserModerator = await this.AccountService.IsInRole(userId, Enum.GetName(typeof(Roles), Roles.Moderator));
            var commentReports = (await this.commentReportReposetory.Get(rep => (rep.Comment.UserId != userId), new string[] { "Comment.Post.Comments", "User", "Comment.User" })).OrderBy(report => report.Date).ToList();
            var notAllowedCommentReports = new List<CommentReport>();
            foreach (CommentReport report in commentReports)
            {
                var isRepUserModerator = await this.AccountService.IsInRole(report.Comment.User.Id, Enum.GetName(typeof(Roles), Roles.Moderator), report.Comment.User);
                if (isCurUserModerator && isRepUserModerator)
                {
                    notAllowedCommentReports.Add(report);
                }
            }
            return commentReports.Except(notAllowedCommentReports).ToList();
        }

        public async Task<bool> RemovePostReport(string reportId)
        {
            PostReport report = (await this.AdminReposetry.Get(report => report.Id == reportId)).SingleOrDefault();
            if (report == null)
            {
                return false;
            }
            await this.AdminReposetry.Remove(report);
            return true;
        }

        public async Task<bool> RemoveCommentReport(string reportId)
        {
            CommentReport report = (await this.commentReportReposetory.Get(report => report.Id == reportId)).SingleOrDefault();
            if (report == null)
            {
                return false;
            }
            await this.commentReportReposetory.Remove(report);
            return true;
        }

        public async Task<bool> PostReportDecision(string reportId, bool isBanUser, bool isDeletePost, bool isHidePost)
        {
            PostReport report = (await this.AdminReposetry.Get(report => report.Id == reportId, new string[] { "Post.User" })).SingleOrDefault();
            if (report == null || report.Post == null)
            {
                return false;
            }
            if (isBanUser)
            {
                var result = await this.AccountService.SetBanRole(report.Post.UserId, report.Post.User);
                if (!result)
                {
                    return false;
                }
            }
            if (isDeletePost)
            {
                await this.PostService.DeletePost(report.PostId, report.Post);
                return true;
            }
            if (isHidePost)
            {
                var result = await this.PostService.HidePost(report.Post.Id);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> CommentReportDecision(string reportId, bool isHideComment, bool isDeleteComment, bool isBanUser, bool isDeletePost, bool isHidePost)
        {
            CommentReport report = (await this.commentReportReposetory.Get(report => report.Id == reportId, new string[] { "Comment", "Comment.Post" })).SingleOrDefault();
            if (report == null || report.Comment == null || report.Comment.Post == null)
            {
                return false;
            }
            if (isBanUser)
            {
                var result = await this.AccountService.SetBanRole(report.Comment.UserId);
                if (!result)
                {
                    return false;
                }
            }
            if (isDeletePost)
            {
                await this.PostService.DeletePost(report.Comment.Post.Id, report.Comment.Post);
                return true;
            }
            if (isHidePost)
            {
                var result = await this.PostService.HidePost(report.Comment.Post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if (isDeleteComment)
            {
                await this.PostService.RemoveComment(report.CommentId);
                return true;
            }
            if (isHideComment)
            {
                var result = await this.PostService.HideComment(report.CommentId);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<ICollection<string>> GetUserRoles(ApplicationUser user)
        {
            return await this.AccountService.GetUserRoles(user);
        }

        public async Task<List<ApplicationUser>> GetUsersAndRoles()
        {
            IList<ApplicationUser> allUsers = this.AccountService.GetAllUsers();
            IList<ApplicationUser> adminUser = new List<ApplicationUser>();
            foreach (ApplicationUser user in allUsers)
            {
                if (await this.AccountService.IsInRole(user.Id, Enum.GetName(typeof(Roles), Roles.Admin), user))
                {
                    adminUser.Add(user);
                }
            }
            return allUsers.Except(adminUser).ToList();
        }
        public async Task<bool> AddModeratorRoots(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            return user == null ? false : await this.AccountService.SetModerator(user);
        }

        public async Task<bool> RemoveModeratorRoots(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            return user == null ? false : await this.AccountService.SetNonModerator(user);
        }
    }
}
