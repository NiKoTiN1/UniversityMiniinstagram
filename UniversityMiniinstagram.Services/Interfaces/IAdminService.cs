using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAdminService
    {
        public Task ReportComment(string userId, string commentId);
        public Task ReportPost(string postId, string userId);
        public Task<ICollection<PostReport>> GetPostReports(string userId);
        public Task<List<CommentReport>> GetCommentReports(string userId);
        public Task<bool> RemovePostReport(string reportId);
        public Task<bool> RemoveCommentReport(string reportId);
        public Task<bool> PostReportDecision(string reportId, bool isBanUser, bool isDeletePost, bool isHidePost);
        public Task<bool> CommentReportDecision(string reportId, bool isHideComment, bool isDeleteComment, bool isBanUser, bool isDeletePost, bool isHidePost);
        public Task<ICollection<string>> GetUserRoles(ApplicationUser user);
        public Task<List<ApplicationUser>> GetUsersAndRoles();
        public Task<bool> AddModeratorRoots(string userId);
        public Task<bool> RemoveModeratorRoots(string userId);
    }
}
