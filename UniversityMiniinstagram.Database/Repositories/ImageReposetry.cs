using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class ImageReposetry : BaseReposetry<Image>, IImageReposetry
    {
        public ImageReposetry(DatabaseContext context)
            : base(context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;
    }
}
