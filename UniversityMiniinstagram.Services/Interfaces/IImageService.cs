using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IImageService
    {
        public Task<Image> Add(ImageViewModel vm, string rootPath);
        public Image GetImage(Guid imageId);
        public void RemoveImage(Image image, DatabaseContext db = null);
    }
}
