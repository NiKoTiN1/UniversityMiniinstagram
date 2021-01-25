using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.Views;

namespace UniversityMiniinstagram.Services
{
    public class ImageService : IImageService
    {
        public ImageService(IImageRepository imageReposetry, IConfiguration configuration)
        {
            this.ImageReposetry = imageReposetry;
            Configuration = configuration;
        }

        private readonly IImageRepository ImageReposetry;
        private IConfiguration Configuration { get; }

        public async Task<Image> Add(ImageViewModel vm, string rootPath)
        {
            if (vm.File == null)
            {
                return null;
            }
            var imageGuid = Guid.NewGuid().ToString();
            var path = Configuration["Storage:Folder:Images"] + imageGuid + '.' + vm.File.FileName.Split(".").Last();

            using (var fileStream = new FileStream(rootPath + path, FileMode.Create, FileAccess.Write))
            {
                await vm.File.CopyToAsync(fileStream);
            }
            var image = new Image()
            {
                Id = imageGuid,
                Path = path,
                UploadDate = DateTime.Now,
            };
            await this.ImageReposetry.Add(image);
            return image;
        }

        public void RemoveImage(Image image)
        {
            var path = "wwwroot/" + image.Path;
            if (!File.Exists(path))
            {
                return;
            }
            File.Delete(path);
        }
    }
}
