﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Interfases;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services
{
    public class ImageService : IImageService
    {
        public ImageService(IImageReposetry imageReposetry, IConfiguration configuration)
        {
            _imageReposetry = imageReposetry;
            Configuration = configuration;
        }

        IImageReposetry _imageReposetry;
        IConfiguration Configuration { get; }

        public async Task<Image> Add(ImageViewModel vm, string rootPath)
        {
            if (vm.File != null)
            {
                Guid imageGuid = Guid.NewGuid();
                var ext = vm.File.FileName.Split(".").Last();
                string path = Configuration["Storage:Folder:Images"] + imageGuid + '.' + ext;

                using (var fileStream = new FileStream(rootPath + path, FileMode.Create, FileAccess.Write))
                {
                    await vm.File.CopyToAsync(fileStream);
                }
                Image image = new Image()
                {
                    Id = imageGuid,
                    Path = path,
                    UploadDate = DateTime.Now,
                };
                _imageReposetry.AddImage(image);
                return image;
            }
            else
                return null ;
        }

        public Image GetImage(Guid imageId)
        {
            var image = _imageReposetry.GetImage(imageId);
            return image;
        }

        public void RemoveImage(Image image, DatabaseContext db=null)
        {
            if (File.Exists("wwwroot/" + image.Path))
            {
                File.Delete("wwwroot/" + image.Path);
            }
            _imageReposetry.RemoveImage(image, db);
        }
    }
}
