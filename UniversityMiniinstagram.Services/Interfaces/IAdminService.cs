using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAdminService
    {
        public Task ReportComment(SendReportViewModel vm);
        public Task ReportPost(SendReportViewModel vm);
        public ICollection<Report> GetPostReports();
        public Task<ICollection<Report>> GetCommentReports();
        public Task<bool> RemoveReport(string reportId);
        public Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm);
        public Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm);
        public Task<List<UserRolesViewModel>> GetUsersAndRoles();
        public Task<bool> AddModeratorRoots(string userId);
        public Task<bool> RemoveModeratorRoots(string userId);
        public Task<bool> IsModerateAllowed(string curUserId, string reportUserId);
    }
}
