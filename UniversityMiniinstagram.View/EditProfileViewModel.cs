using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public IFormFile File { get; set; }
        public Image Avatar { get; set; }
        public bool HasPassword { get; set; }
    }
}
