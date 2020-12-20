using System;

namespace UniversityMiniinstagram.View
{
    public class SendReportViewModel
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}
