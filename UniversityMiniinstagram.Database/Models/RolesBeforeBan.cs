using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityMiniinstagram.Database.Models
{
    public class RolesBeforeBan
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public IdentityRole Role { get; set; }
        public string RoleId { get; set; }
    }
}
