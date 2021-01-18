using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(DatabaseContext context)
            : base(context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;

        public bool IsPostReported(string postId, string userId)
        {
            var result = this.Context.PostReports.Where(report => report.PostId == postId && report.UserId == userId).ToList();
            return result.Count != 0;
        }
    }
}
