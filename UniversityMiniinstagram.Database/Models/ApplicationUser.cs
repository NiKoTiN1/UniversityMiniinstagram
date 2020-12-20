using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace UniversityMiniinstagram.Database.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Description { get; set; }
        public Image Avatar { get; set; }
        public Guid? AvatarId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
