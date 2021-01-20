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

        public async Task ReportComment(SendReportViewModel vm)
        {
            await this.commentReportReposetory.Add(new CommentReport() { Id = Guid.NewGuid().ToString(), CommentId = vm.CommentId, UserId = vm.UserId, Date = DateTime.UtcNow });
        }

        public async Task ReportPost(SendReportViewModel vm)
        {
            await this.AdminReposetry.Add(new PostReport() { Id = Guid.NewGuid().ToString(), PostId = vm.PostId, UserId = vm.UserId, Date = DateTime.UtcNow });
        }

        public async Task<ICollection<AdminPostReportsVeiwModel>> GetPostReports(string userId)
        {
            var vmList = new List<AdminPostReportsVeiwModel>();
            var result = (await this.AdminReposetry.Get(report => true, new string[] { "Post.Comments.User", "Post.Likes" })).OrderBy(report => report.Date).ToList();
            foreach (PostReport report in result)
            {
                report.Post = await this.PostService.GetPost(report.PostId);
                if (report.Post.UserId != userId)
                {
                    var vm = new AdminPostReportsVeiwModel
                    {
                        Report = report,
                        CommentViewModel = new List<CommentViewModel>()
                    };
                    foreach (Comment comment in report.Post.Comments.OrderBy(comment => comment.Date))
                    {
                        var commVm = new CommentViewModel()
                        {
                            Comment = comment,
                            IsDeleteAllowed = false,
                            IsReportAllowed = false,
                            ShowReportColor = false
                        };
                        vm.CommentViewModel.Add(commVm);
                    }
                    vmList.Add(vm);
                }
            }
            return vmList;
        }

        public async Task<List<AdminCommentReportsVeiwModel>> GetCommentReports(string userId)
        {
            var isCurUserModerator = await this.AccountService.IsInRole(userId, "Moderator");
            var commentReports = (await this.commentReportReposetory.Get(rep => (rep.Comment.UserId != userId), new string[] { "Comment.Post.Comments", "User", "Comment.User" })).OrderBy(report => report.Date).ToList();
            var commentReportsVM = new List<AdminCommentReportsVeiwModel>();
            foreach (CommentReport report in commentReports)
            {
                var isRepUserModerator = await this.AccountService.IsInRole(report.Comment.User.Id, "Moderator", report.Comment.User);
                if (!(isCurUserModerator && isRepUserModerator))
                {
                    var commentReportVM = new AdminCommentReportsVeiwModel()
                    {
                        Report = report,
                        CommentViewModel = report.Comment.Post.Comments.Select(comment => new CommentViewModel()
                        {
                            Comment = comment,
                            IsDeleteAllowed = false,
                            IsReportAllowed = false,
                            ShowReportColor = report.CommentId == comment.Id
                        }).ToList()
                    };
                    commentReportsVM.Add(commentReportVM);
                }
            }
            return commentReportsVM.ToList();
        }

        public async Task<bool> RemovePostReport(string reportId)
        {
            PostReport report = (await this.AdminReposetry.Get(report => report.Id == reportId)).SingleOrDefault();
            if (report != null)
            {
                await this.AdminReposetry.Remove(report);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveCommentReport(string reportId)
        {
            CommentReport report = (await this.commentReportReposetory.Get(report => report.Id == reportId)).SingleOrDefault();
            if (report != null)
            {
                await this.commentReportReposetory.Remove(report);
                return true;
            }
            return false;
        }

        public async Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            PostReport report = (await this.AdminReposetry.Get(report => report.Id == vm.ReportId, new string[] { "Post.User" })).SingleOrDefault();
            if (report == null || report.Post == null)
            {
                return false;
            }
            if (vm.IsBanUser)
            {
                var result = await this.AccountService.SetBanRole(report.Post.UserId, report.Post.User);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeletePost)
            {
                await this.PostService.DeletePost(report.PostId, report.Post);
                return true;
            }
            if (vm.IsHidePost)
            {
                var result = await this.PostService.HidePost(report.Post.Id);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            CommentReport report = (await this.commentReportReposetory.Get(report => report.Id == vm.ReportId, new string[] { "Comment", "Comment.Post" })).SingleOrDefault();
            if (report == null || report.Comment == null || report.Comment.Post == null)
            {
                return false;
            }
            if (vm.IsBanUser)
            {
                var result = await this.AccountService.SetBanRole(report.Comment.UserId);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeletePost)
            {
                await this.PostService.DeletePost(report.Comment.Post.Id, report.Comment.Post);
                return true;
            }
            if (vm.IsHidePost)
            {
                var result = await this.PostService.HidePost(report.Comment.Post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeleteComment)
            {
                await this.PostService.RemoveComment(report.CommentId);
                return true;
            }
            if (vm.IsHideComment)
            {
                var result = await this.PostService.HideComment(report.CommentId);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<List<UserRolesViewModel>> GetUsersAndRoles()
        {
            IList<ApplicationUser> allUsers = this.AccountService.GetAllUsers();
            var vmList = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in allUsers)
            {
                if (!await this.AccountService.IsInRole(user.Id, "Admin", user))
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
    }
}
