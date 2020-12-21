using System;
using System.Linq;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class ImageReposetry : IImageReposetry
    {
        public ImageReposetry(DatabaseContext context)
        {
            Context = context;
        }

        private readonly DatabaseContext Context;
        public void AddImage(Image image)
        {
            Context.Images.Add(image);
            Context.SaveChanges();
        }

        public Image GetImage(Guid imageId)
        {
            Image image = Context.Images.FirstOrDefault(image => image.Id == imageId);
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
            Context.Remove(image);
            Context.SaveChanges();
        }
    }
}
