using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class UserRolesViewModel
    {
        public ICollection<string> UserRoles { get; set; }
        public ApplicationUser User { get; set; }
    }
}
