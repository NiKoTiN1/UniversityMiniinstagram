using System;
using System.Collections.Generic;
using System.Text;
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
        public bool RemoveReport(Guid reportId);
        public Task<bool> PostReportDecision(AdminPostReportDecisionViewModel vm);
    }
}
