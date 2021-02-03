using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }

        public bool IsDeleteAllowed { get; set; }

        public bool IsReportAllowed { get; set; }

        public bool ShowReportColor { get; set; }

    }
}
