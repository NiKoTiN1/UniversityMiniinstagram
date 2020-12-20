using System;

namespace UniversityMiniinstagram.View
{
    public class AdminPostReportDecisionViewModel
    {
        public bool IsBanUser { get; set; }
        public bool IsDeletePost { get; set; }
        public bool IsHidePost { get; set; }
        public Guid ReportId { get; set; }
    }
}
