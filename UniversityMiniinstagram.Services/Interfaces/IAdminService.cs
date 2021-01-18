using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAdminService
    {
        public Task ReportComment(SendReportViewModel vm);
        public Task ReportPost(SendReportViewModel vm);
        public Task<ICollection<AdminPostReportsVeiwModel>> GetPostReports(string userId);
        public Task<List<AdminCommentReportsVeiwModel>> GetCommentReports(string userId);
        public Task<bool> RemovePostReport(string reportId);
        public Task<bool> RemoveCommentReport(string reportId);
        public Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm);
        public Task<bool> CommentReportDecision(AdminCommentReportDecisionViewModel vm);
        public Task<List<UserRolesViewModel>> GetUsersAndRoles();
        public Task<bool> AddModeratorRoots(string userId);
        public Task<bool> RemoveModeratorRoots(string userId);
    }
}
