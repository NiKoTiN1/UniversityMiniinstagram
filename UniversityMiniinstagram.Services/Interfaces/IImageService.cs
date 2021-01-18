using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IImageService
    {
        public Task<Image> Add(ImageViewModel vm, string rootPath);
        public void RemoveImage(Image image);
    }
}
