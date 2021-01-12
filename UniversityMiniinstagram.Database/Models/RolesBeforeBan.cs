using Microsoft.AspNetCore.Identity;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class RolesBeforeBan : IEntity
    {
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public IdentityRole Role { get; set; }
        public string RoleId { get; set; }
    }
}
