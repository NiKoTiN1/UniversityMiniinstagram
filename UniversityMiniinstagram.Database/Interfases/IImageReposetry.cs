using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityMiniinstagram.Database.Interfases
{
    public interface IImageReposetry
    {
        public void AddImage(Image image);
        public Image GetImage(Guid imageId);

    }
}
