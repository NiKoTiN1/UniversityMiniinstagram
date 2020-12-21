using System;

namespace UniversityMiniinstagram.Views
{
    public class AdminPostReportDecisionViewModel
    {
        public bool IsBanUser { get; set; }
        public bool IsDeletePost { get; set; }
        public bool IsHidePost { get; set; }
        public Guid ReportId { get; set; }
    }
}
