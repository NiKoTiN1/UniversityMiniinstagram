using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class LikeReposetry : BaseReposetry<Like>, ILikeReposetry
    {
        public LikeReposetry(DatabaseContext context)
            : base(context)
        {
            this.dbContext = context;
        }
        private readonly DatabaseContext dbContext;

        public async Task Remove(string postId, string userId)
        {
            Like like = this.dbContext.Likes.FirstOrDefault(li => li.PostId == postId && li.UserId == userId);
            this.dbContext.Likes.Remove(like);
            await this.dbContext.SaveChangesAsync();
        }
        public new ICollection<Like> Get(string postId)
        {
            ICollection<Like> likes = this.dbContext.Likes.Where(like => like.PostId == postId).ToList();
            return likes;
        }
    }
}
