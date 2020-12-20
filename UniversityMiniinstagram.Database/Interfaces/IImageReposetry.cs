using System;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IImageReposetry
    {
        public void AddImage(Image image);
        public Image GetImage(Guid imageId);
        public void RemoveImage(Image image, DatabaseContext db = null);
    }
}
