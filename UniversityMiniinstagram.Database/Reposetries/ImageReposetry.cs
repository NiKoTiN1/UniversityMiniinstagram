using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityMiniinstagram.Database.Interfases;

namespace UniversityMiniinstagram.Database.Reposetries
{
    public class ImageReposetry : IImageReposetry
    {
        public ImageReposetry(DatabaseContext context)
        {
            _context = context;
        }

        DatabaseContext _context;
        public void AddImage(Image image)
        {
            _context.Images.Add(image);
            _context.SaveChanges();
        }

        public Image GetImage(Guid imageId)
        {
            var image = _context.Images.FirstOrDefault(image => image.Id == imageId);
            return image;
        }
    }
}
