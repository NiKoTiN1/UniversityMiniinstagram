using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class IsInRoleViewModel
    {
        public ApplicationUser user { get; set; }
        public string roleName { get; set; }
    }
}
