using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services
{
    public class AdminService : IAdminService
    {
        public AdminService(IAdminRepository adminReposetry, IPostService postService, IAccountService accountService)
        {
            this.AdminReposetry = adminReposetry;
            this.PostService = postService;
            this.AccountService = accountService;
        }

        private readonly IAdminRepository AdminReposetry;
        private readonly IPostService PostService;
        private readonly IAccountService AccountService;

        public async Task ReportComment(SendReportViewModel vm)
        {
            await this.AdminReposetry.Add(new Report() { Id = Guid.NewGuid().ToString(), CommentId = vm.CommentId, UserId = vm.UserId, Date = DateTime.UtcNow });
        }

        public async Task ReportPost(SendReportViewModel vm)
        {
            await this.AdminReposetry.Add(new Report() { Id = Guid.NewGuid().ToString(), PostId = vm.PostId, UserId = vm.UserId, Date = DateTime.UtcNow });
        }

        public ICollection<Report> GetPostReports()
        {
            ICollection<Report> result = this.AdminReposetry.GetPostReports();
            return result;
        }

        public async Task<ICollection<Report>> GetCommentReports()
        {
            ICollection<Report> result = this.AdminReposetry.GetCommentReports();
            foreach (Report report in result)
            {
                report.Comment = await this.PostService.GetComment(report.CommentId);
            }
            return result;
        }

        public async Task<bool> RemoveReport(string reportId)
        {
            Report report = await this.AdminReposetry.Get(reportId);
            if (report != null)
            {
                await this.AdminReposetry.Remove(report);
                return true;
            }
            return false;
        }

        public async Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            Report report = await this.AdminReposetry.Get(vm.ReportId);
            if (report == null)
            {
                return false;
            }
            Post post = await this.PostService.GetPost(report.PostId);
            if (post == null)
            {
                return false;
            }
            if (vm.IsHidePost)
            {
                var result = await this.PostService.HidePost(post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsBanUser)
            {
                ApplicationUser user = await this.AccountService.GetUser(report.Post.UserId);
                var result = await this.AccountService.SetBanRole(user);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeletePost)
            {
                await this.PostService.DeletePost(post);
            }
            return true;
        }

        public async Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            Report report = await this.AdminReposetry.Get(vm.ReportId);
            if (report == null)
            {
                return false;
            }
            Comment comment = await this.PostService.GetComment(report.CommentId);
            if (comment == null)
            {
                return false;
            }
            Post post = await this.PostService.GetPost(comment.PostId);
            if (post == null)
            {
                return false;
            }
            if (vm.IsHidePost)
            {
                var result = await this.PostService.HidePost(post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsHideComment)
            {
                var result = await this.PostService.HideComment(report.CommentId);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsBanUser)
            {
                ApplicationUser user = await this.AccountService.GetUser(report.Comment.UserId);
                var result = await this.AccountService.SetBanRole(user);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeleteComment)
            {
                await this.PostService.RemoveComment(report.CommentId);
            }
            if (vm.IsDeletePost)
            {
                await this.PostService.DeletePost(post);
            }
            return true;
        }
        public async Task<List<UserRolesViewModel>> GetUsersAndRoles()
        {
            IList<ApplicationUser> allUsers = await this.AccountService.GetAllUsers();
            var vmList = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in allUsers)
            {
                if (!await this.AccountService.IsInRole(new IsInRoleViewModel() { User = user, RoleName = "Admin" }))
                {
                    ICollection<string> userRoles = await this.AccountService.GetUserRoles(user);
                    var vm = new UserRolesViewModel()
                    {
                        User = user,
                        UserRoles = userRoles
                    };
                    vmList.Add(vm);
                }
            }
            return vmList;
        }
        public async Task<bool> AddModeratorRoots(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            if (user == null)
            {
                return false;
            }
            var result = await this.AccountService.SetModerator(user);
            return result;
        }

        public async Task<bool> RemoveModeratorRoots(string userId)
        {
            ApplicationUser user = await this.AccountService.GetUser(userId);
            if (user == null)
            {
                return false;
            }
            var result = await this.AccountService.SetNonModerator(user);
            return result;
        }
        public async Task<bool> IsModerateAllowed(string curUserId, string reportUserId)
        {
            ApplicationUser curUser = await this.AccountService.GetUser(curUserId);
            ApplicationUser reportUser = await this.AccountService.GetUser(reportUserId);
            var result1 = await this.AccountService.IsInRole(new IsInRoleViewModel() { User = curUser, RoleName = "Moderator" });
            var result2 = await this.AccountService.IsInRole(new IsInRoleViewModel() { User = reportUser, RoleName = "Moderator" });
            return !(result1 && result2);
        }
    }
}
