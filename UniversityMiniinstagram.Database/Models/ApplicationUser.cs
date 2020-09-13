﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Database
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; }
        public Image Avatar { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
