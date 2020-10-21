using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityMiniinstagram.View
{
    public class EditProfileViewModel: ImageViewModel
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public string WebRootPath { get; set; }
        public string Userid { get; set; }
    }
}
