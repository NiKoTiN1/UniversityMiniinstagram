using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class AdminPostReportsVeiwModel
    {
        public Report Report { get; set; }
        public ICollection<CommentViewModel> CommentViewModel { get; set; }
    }
}
