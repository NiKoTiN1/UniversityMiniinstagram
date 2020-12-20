using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public bool IsDeleteRelated { get; set; }
        public bool IsReportRelated { get; set; }
        public bool ShowReportColor { get; set; }

    }
}
