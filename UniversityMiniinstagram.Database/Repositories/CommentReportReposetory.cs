using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class CommentReportReposetory : BaseRepository<CommentReport>, ICommentReportReposetory
    {
        public CommentReportReposetory(DatabaseContext context)
            : base(context)
        {

        }
    }
}
