using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public bool IsDeleteRelated { get; set; }
        public bool IsReportRelated { get; set; }

    }
}
