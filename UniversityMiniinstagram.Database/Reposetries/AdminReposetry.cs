using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class AdminReposetry : IAdminReposetry
    {
        public AdminReposetry(DatabaseContext context)
        {
            _context = context;
        }

        DatabaseContext _context;
        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
            _context.SaveChanges();
        }

        public bool IsCommentReported(Guid commentId, string userId)
        {
            var result = _context.Reports.Where(report => report.CommentId == commentId && report.UserId == userId).ToList();
            if(result.Count == 0)
            {
                return false;
            }
            return true;
        }

        public bool IsPostReported(Guid postId, string userId)
        {
            var result = _context.Reports.Where(report => report.PostId == postId && report.UserId == userId).ToList();
            if (result.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
