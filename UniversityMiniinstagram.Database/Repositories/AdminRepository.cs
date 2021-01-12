using System.Collections.Generic;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AdminRepository : BaseRepository<Report>, IAdminRepository
    {
        public AdminRepository(DatabaseContext context)
            : base(context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;

        public ICollection<Report> GetPostReports()
        {
            var result = this.Context.Reports.Where(report => report.PostId != null).OrderBy(report => report.Date).ToList();
            return result;
        }

        public ICollection<Report> GetCommentReports()
        {
            var result = this.Context.Reports.Where(report => report.CommentId != null).OrderBy(report => report.Date).ToList();
            return result;
        }
    }
}
