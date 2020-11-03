using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityMiniinstagram.View
{
    public class AdminCommentReportDecisionViewModel : AdminPostReportDecisionViewModel
    {
        public bool IsHideComment { get; set; }
        public bool IsDeleteComment { get; set; }
    }
}
