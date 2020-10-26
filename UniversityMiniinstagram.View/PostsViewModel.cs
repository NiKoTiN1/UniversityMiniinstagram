using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public ICollection<CommentViewModel> vm { get; set; }
        public bool IsReportRelated { get; set; }
    }
}
