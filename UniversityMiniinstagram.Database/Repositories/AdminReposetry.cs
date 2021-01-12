using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AdminReposetry : BaseReposetry<Report>, IAdminReposetry
    {
        public AdminReposetry(DatabaseContext context)
            : base(context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;

        public async Task<ICollection<Report>> GetPostReports()
        {
            List<Report> result = await this.Context.Reports.Where(report => report.PostId != null).OrderBy(report => report.Date).ToListAsync();
            return result;
        }

        public async Task<ICollection<Report>> GetCommentReports()
        {
            List<Report> result = await this.Context.Reports.Where(report => report.CommentId != null).OrderBy(report => report.Date).ToListAsync();
            return result;
        }
    }
}
