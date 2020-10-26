using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IAdminService
    {
        public void ReportComment(SendReportViewModel vm);
        public void ReportPost(SendReportViewModel vm);
        public bool IsPostReported(Guid postId, string userId);
        public bool IsCommentReported(Guid commentId, string userId);
    }
}
