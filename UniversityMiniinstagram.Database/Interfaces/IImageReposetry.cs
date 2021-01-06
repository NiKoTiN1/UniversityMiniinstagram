using System;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IImageReposetry
    {
        public Task AddImage(Image image);
        public Image GetImage(Guid imageId);
        public Task RemoveImage(Image image, DatabaseContext db = null);
    }
}
