﻿using System;
using System.Threading.Tasks;
using UniversityMiniinstagram.Database;
using UniversityMiniinstagram.Database.Models;
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
