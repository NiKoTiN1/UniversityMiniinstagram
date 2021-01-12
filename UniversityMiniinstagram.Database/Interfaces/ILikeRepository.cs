using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        public new ICollection<Like> Get(string postId);
        public Task Remove(string postId, string userId);
    }
}
