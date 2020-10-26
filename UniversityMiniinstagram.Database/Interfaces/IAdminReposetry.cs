using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IAdminReposetry
    {
        public void AddReport(Report report);
        public bool IsCommentReported(Guid commentId, string userId);
        public bool IsPostReported(Guid postId, string userId);
    }
}
