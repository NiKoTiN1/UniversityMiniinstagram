using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class PostsViewModel
    {
        public Post Post { get; set; }

        public ICollection<CommentViewModel> CommentVM { get; set; }

        public bool IsReportAllowed { get; set; }

        public bool IsDeleteAllowed { get; set; }
    }
}
