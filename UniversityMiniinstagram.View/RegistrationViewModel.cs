using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Views
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not correct")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Description { get; set; }
    }
}
