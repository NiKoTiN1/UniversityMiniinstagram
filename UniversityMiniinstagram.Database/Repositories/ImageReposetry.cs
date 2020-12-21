using System;
using System.Linq;
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
        public void AddImage(Image image)
        {
            this.Context.Images.Add(image);
            this.Context.SaveChanges();
        }

        public Image GetImage(Guid imageId)
        {
            Image image = this.Context.Images.FirstOrDefault(image => image.Id == imageId);
            return image;
        }

        public void RemoveImage(Image image, DatabaseContext db = null)
        {
            if (db != null)
            {
                db.Remove(image);
                db.SaveChanges();
                return;
            }
            this.Context.Remove(image);
            this.Context.SaveChanges();
        }
    }
}
