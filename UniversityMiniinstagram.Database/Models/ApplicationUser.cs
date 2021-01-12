using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class ApplicationUser : IdentityUser, IEntity
    {
        public string Description { get; set; }
        public Image Avatar { get; set; }
        public string? AvatarId { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
