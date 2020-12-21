using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class IsInRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
    }
}
