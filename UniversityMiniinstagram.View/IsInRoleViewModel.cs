using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class IsInRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
    }
}
