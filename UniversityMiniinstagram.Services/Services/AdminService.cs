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
        public AdminService(IAdminReposetry adminReposetry, IPostService postService, IAccountService accountService)
        {
            this.AdminReposetry = adminReposetry;
            this.PostService = postService;
            this.AccountService = accountService;
        }

        private readonly IAdminReposetry AdminReposetry;
        private readonly IPostService PostService;
        private readonly IAccountService AccountService;

        public async Task ReportComment(SendReportViewModel vm)
        {
            await this.AdminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), CommentId = vm.CommentId, UserId = vm.UserId });
        }

        public async Task ReportPost(SendReportViewModel vm)
        {
            await this.AdminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), PostId = vm.PostId, UserId = vm.UserId });
        }

        public ICollection<Report> GetPostReports()
        {
            ICollection<Report> result = this.AdminReposetry.GetPostReports();
            return result;
        }

        public ICollection<Report> GetCommentReports()
        {
            ICollection<Report> result = this.AdminReposetry.GetCommentReports();
            foreach (Report report in result)
            {
                report.Comment = this.PostService.GetComment(report.CommentId.Value);
            }
            return result;
        }

        public async Task<bool> RemoveReport(Guid reportId)
        {
            Report report = this.AdminReposetry.GetReport(reportId);
            if (report != null)
            {
                await this.AdminReposetry.RemoveReport(report);
                return true;
            }
            return false;
        }

        public async Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            Report report = this.AdminReposetry.GetReport(vm.ReportId);
            if (report == null)
            {
                return false;
            }
            Post post = await this.PostService.GetPost(report.PostId.Value);
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
            Report report = this.AdminReposetry.GetReport(vm.ReportId);
            if (report == null)
            {
                return false;
            }
            Comment comment = this.PostService.GetComment(report.CommentId.Value);
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
                var result = await this.PostService.HideComment(report.CommentId.Value);
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
                await this.PostService.RemoveComment(report.CommentId.Value);
            }
            if (vm.IsDeletePost)
            {
                await this.PostService.DeletePost(post);
            }
            return true;
        }
        public async Task<List<UserRolesViewModel>> GetUsersAndRoles()
        {
            IList<ApplicationUser> allUsers = this.AccountService.GetAllUsers();
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
