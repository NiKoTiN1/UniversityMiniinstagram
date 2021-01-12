using System.Collections.Generic;
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

        public ICollection<Post> GetUserPosts(string userId)
        {
            var userPosts = this.Context.Posts.Where(post => post.UserId == userId).OrderBy(post => post.UploadDate).ToList();
            return userPosts;
        }
        public bool IsPostReported(string postId, string userId)
        {
            var result = this.Context.Reports.Where(report => report.PostId == postId && report.UserId == userId).ToList();
            return result.Count != 0;
        }
    }
}
