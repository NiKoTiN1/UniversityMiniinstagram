﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class AdminService : IAdminService
    {
        public AdminService(IAdminReposetry adminReposetry, IPostService postService, IAccountService accountService)
        {
            _adminReposetry = adminReposetry;
            _postService = postService;
            _accountService = accountService;
        }

        IAdminReposetry _adminReposetry;
        IPostService _postService;
        IAccountService _accountService;

        public void ReportComment(SendReportViewModel vm)
        {
            _adminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), CommentId = vm.CommentId, UserId = vm.UserId });
        }

        public void ReportPost(SendReportViewModel vm)
        {
            _adminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), PostId = vm.PostId, UserId = vm.UserId });
        }

        public ICollection<Report> GetPostReports()
        {
            var result = _adminReposetry.GetPostReports();
            return result;
        }

        public ICollection<Report> GetCommentReports()
        {
            var result = _adminReposetry.GetCommentReports();
            foreach(var report in result)
            {
                report.Comment = _postService.GetComment(report.CommentId.Value);
            }
            return result;
        }

        public bool RemoveReport(Guid reportId)
        {
            var report = _adminReposetry.GetReport(reportId);
            if (report != null)
            {
                _adminReposetry.RemoveReport(report);
                return true;
            }
            return false;
        }

        public async Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm)
        {
            var report = _adminReposetry.GetReport(vm.ReportId);
            if(report == null)
            {
                return false;
            }
            var post = await _postService.GetPost(report.PostId.Value);
            if(post == null)
            {
                return false;
            }
            if (vm.IsHidePost)
            {
                var result = _postService.HidePost(post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsBanUser)
            {
                var user = await _accountService.GetUser(report.Post.UserId);
                var result = await _accountService.SetBanRole(user);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeletePost)
            {
                _postService.DeletePost(post);
            }
            return true;
        }

        public async Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm)
        {
            var report = _adminReposetry.GetReport(vm.ReportId);
            if (report == null)
            {
                return false;
            }
            var comment = _postService.GetComment(report.CommentId.Value);
            if(comment == null)
            {
                return false;
            }
            var post = await _postService.GetPost(comment.PostId);
            if (post == null)
            {
                return false;
            }
            if (vm.IsHidePost)
            {
                var result = _postService.HidePost(post.Id);
                if (!result)
                {
                    return false;
                }
            }
            if(vm.IsHideComment)
            {
                var result = _postService.HideComment(report.CommentId.Value);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsBanUser)
            {
                var user = await _accountService.GetUser(report.Comment.UserId);
                var result = await _accountService.SetBanRole(user);
                if (!result)
                {
                    return false;
                }
            }
            if (vm.IsDeleteComment)
            {
                _postService.RemoveComment(report.CommentId.Value);
            }
            if (vm.IsDeletePost)
            {
                _postService.DeletePost(post);
            }
            return true;
        }
        public async Task<List<UserRolesViewModel>> GetUsersAndRoles ()
        {
            var allUsers = _accountService.GetAllUsers();
            List<UserRolesViewModel> vmList = new List<UserRolesViewModel>();
            foreach(var user in allUsers)
            {
                if(!await _accountService.IsInRole(new IsInRoleViewModel() { user = user, roleName = "Admin" }))
                {
                    var userRoles = await _accountService.GetUserRoles(user);
                    UserRolesViewModel vm = new UserRolesViewModel()
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
            var user = await _accountService.GetUser(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _accountService.SetModerator(user);
            if(!result)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveModeratorRoots(string userId)
        {
            var user = await _accountService.GetUser(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _accountService.SetNonModerator(user);
            if (!result)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> isModerateAllowed(string curUserId, string reportUserId)
        {
            var curUser = await _accountService.GetUser(curUserId);
            var reportUser = await _accountService.GetUser(reportUserId);
            var result1 = await _accountService.IsInRole(new IsInRoleViewModel() { user = curUser, roleName = "Modarator" });
            var result2 = await _accountService.IsInRole(new IsInRoleViewModel() { user = reportUser, roleName = "Modarator" });
            return !(result1 && result2);
        }
    }
}
