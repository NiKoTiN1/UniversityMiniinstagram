using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Services.Interfaces;
using UniversityMiniinstagram.View;

namespace UniversityMiniinstagram.Services
{
    public class ImageServices : IImageService
    {
        public ImageServices(DatabaseContext context)
        {
            _context = context;
        }

        DatabaseContext _context;

        public async Task<Image> Add(ImageViewModel vm, string rootPath)
        {
            if (vm.File != null)
            {
                Guid imageGuid = Guid.NewGuid();
                string path = "/Images/" + vm.File.FileName + '_' + vm.Category + '_' + imageGuid;

                using (var fileStream = new FileStream(rootPath + path, FileMode.Create, FileAccess.Write))
                {
                    await vm.File.CopyToAsync(fileStream);
                }
                Image image = new Image()
                {
                    Id = imageGuid,
                    Path = path,
                    category = vm.Category,
                    UploadDate = DateTime.Now,
                };
                _context.Images.Add(image);
                _context.SaveChanges();
                return image;
            }
            else
                return null ;
        }
    }
}
