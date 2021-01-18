using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class AdminRepository : BaseRepository<PostReport>, IAdminRepository
    {
        public AdminRepository(DatabaseContext context)
            : base(context)
        {

        }
    }
}
