using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfaces;
using UniversityMiniinstagram.Database.Models;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services
{
    public class ImageService : IImageService
    {
        public ImageService(IImageReposetry imageReposetry, IConfiguration configuration)
        {
            this.ImageReposetry = imageReposetry;
            Configuration = configuration;
        }

        private readonly IImageReposetry ImageReposetry;

        private IConfiguration Configuration { get; }

        public async Task<Image> Add(ImageViewModel vm, string rootPath)
        {
            if (vm.File != null)
            {
                var imageGuid = Guid.NewGuid();
                var ext = vm.File.FileName.Split(".").Last();
                var path = Configuration["Storage:Folder:Images"] + imageGuid + '.' + ext;

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
                this.ImageReposetry.AddImage(image);
                return image;
            }
            else
            {
                return null;
            }
        }

        public Image GetImage(Guid imageId)
        {
            Image image = this.ImageReposetry.GetImage(imageId);
            return image;
        }

        public void RemoveImage(Image image, DatabaseContext db = null)
        {
            if (File.Exists("wwwroot/" + image.Path))
            {
                File.Delete("wwwroot/" + image.Path);
            }
            this.ImageReposetry.RemoveImage(image, db);
        }
    }
}
