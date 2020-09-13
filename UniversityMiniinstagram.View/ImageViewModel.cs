using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class ImageViewModel
    { 
        public IFormFile File { get; set; }
        public Category Category { get; set; }
    }
}
