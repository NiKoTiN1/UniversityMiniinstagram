using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class CreatePostViewModel:ImageViewModel
    {
        public string Description { get; set; }
        public Category category { get; set; }
    }
}
