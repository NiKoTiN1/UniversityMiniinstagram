using System;

namespace UniversityMiniinstagram.Views
{
    public class SendReportViewModel
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}
