using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class ApplicationUser : IdentityUser, IEntity
    {

        public ApplicationUser() { }
        public ApplicationUser(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
        }

        private readonly ILazyLoader lazyLoader;

        public string Description { get; set; }

        private Image avatar;
        public Image Avatar
        {
            get => this.lazyLoader.Load(this, ref this.avatar);
            set => this.avatar = value;
        }
    #nullable enable
        public string? AvatarId { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
