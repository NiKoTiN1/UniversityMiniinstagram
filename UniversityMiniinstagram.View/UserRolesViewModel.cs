using System.Collections.Generic;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class UserRolesViewModel
    {
        public ICollection<string> UserRoles { get; set; }
        public ApplicationUser User { get; set; }
    }
}
