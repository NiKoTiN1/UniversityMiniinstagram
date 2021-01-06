using System;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Repositories
{
    public class ImageReposetry : IImageReposetry
    {
        public ImageReposetry(DatabaseContext context)
        {
            this.Context = context;
        }

        private readonly DatabaseContext Context;
        public async Task AddImage(Image image)
        {
            this.Context.Images.Add(image);
            await this.Context.SaveChangesAsync();
        }

        public Image GetImage(Guid imageId)
        {
            Image image = this.Context.Images.FirstOrDefault(image => image.Id == imageId);
            return image;
        }

        public async Task RemoveImage(Image image, DatabaseContext db = null)
        {
            if (db != null)
            {
                db.Remove(image);
                await db.SaveChangesAsync();
                return;
            }
            this.Context.Remove(image);
            this.Context.SaveChanges();
        }
    }
}
