using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class PostsViewModel
    {
        public Post Post { get; set; }
        public ICollection<CommentViewModel> vm { get; set; }
        public bool IsReportRelated { get; set; }
        public bool IsDeleteRelated { get; set; }
    }
}
