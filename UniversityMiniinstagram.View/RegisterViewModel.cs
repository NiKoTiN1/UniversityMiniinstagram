using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public Image Avatar { get; set; }

    }
}
