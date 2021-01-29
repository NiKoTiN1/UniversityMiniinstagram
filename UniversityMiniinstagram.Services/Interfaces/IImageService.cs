using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Services.Interfaces
{
    public interface IImageService
    {
        public Task<Image> Add(IFormFile file, string rootPath);
        public void RemoveImage(Image image);
    }
}
