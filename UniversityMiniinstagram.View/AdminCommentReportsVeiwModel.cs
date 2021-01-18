using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class AdminCommentReportsVeiwModel
    {
        public CommentReport Report { get; set; }
        public ICollection<CommentViewModel> CommentViewModel { get; set; }
    }
}
