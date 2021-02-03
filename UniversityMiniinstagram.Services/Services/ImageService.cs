using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;

namespace UniversityMiniinstagram.Services
{
    public class ImageService : IImageService
    {
        public ImageService(IImageRepository imageReposetry, IConfiguration configuration)
        {
            this.imageReposetry = imageReposetry;
            this.configuration = configuration;
        }

        private readonly IImageRepository imageReposetry;

        private IConfiguration configuration { get; }

        public async Task<Image> Add(IFormFile file, string rootPath)
        {
            if (file == null)
            {
                return null;
            }
            string imageGuid = Guid.NewGuid().ToString();
            string path = configuration["Storage:Folder:Images"] + imageGuid + '.' + file.FileName.Split(".").Last();

            using (var fileStream = new FileStream(rootPath + path, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream).ConfigureAwait(false);
            }
            var image = new Image()
            {
                Id = imageGuid,
                Path = path,
                UploadDate = DateTime.Now,
            };
            await this.imageReposetry.Add(image).ConfigureAwait(false);
            return image;
        }

        public void RemoveImage(Image image)
        {
            string path = "wwwroot/" + image.Path;
            if (!File.Exists(path))
            {
                return;
            }
            File.Delete(path);
        }
    }
}
