using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Services
{
    public class AdminService : IAdminService
    {
        public AdminService(IAdminReposetry adminReposetry)
        {
            _adminReposetry = adminReposetry;
        }

        IAdminReposetry _adminReposetry;

        public void ReportComment(SendReportViewModel vm)
        {
            _adminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), PostId = vm.PostId, CommentId = vm.CommentId, UserId = vm.UserId });
        }

        public void ReportPost(SendReportViewModel vm)
        {
            _adminReposetry.AddReport(new Report() { Id = Guid.NewGuid(), PostId = vm.PostId, UserId = vm.UserId });
        }

        public bool IsPostReported(Guid postId, string userId)
        {
            return _adminReposetry.IsPostReported(postId, userId);
        }
        public bool IsCommentReported(Guid commentId, string userId)
        {
            return _adminReposetry.IsCommentReported(commentId, userId);
        }
    }
}
