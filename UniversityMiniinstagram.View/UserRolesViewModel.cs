using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class UserRolesViewModel
    {
        public ICollection<string> UserRoles { get; set; }
        public ApplicationUser User { get; set; }
    }
}
