using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(DatabaseContext context)
            : base(context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;
    }
}
