using System;
using System.Collections.Generic;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class IsInRoleViewModel
    {
        public ApplicationUser user { get; set; }
        public string roleName { get; set; }
    }
}
