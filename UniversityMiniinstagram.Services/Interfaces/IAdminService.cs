using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAdminService
    {
        public void ReportComment(SendReportViewModel vm);
        public void ReportPost(SendReportViewModel vm);
        public ICollection<Report> GetPostReports();
        public ICollection<Report> GetCommentReports();
        public bool RemoveReport(Guid reportId);
        public Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm);
        public Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm);
        public Task<List<UserRolesViewModel>> GetUsersAndRoles();
        public Task<bool> AddModeratorRoots(string userId);
        public Task<bool> RemoveModeratorRoots(string userId);
        public Task<bool> IsModerateAllowed(string curUserId, string reportUserId);
    }
}
