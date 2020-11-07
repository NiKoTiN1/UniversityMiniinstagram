using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UniversityMiniinstagram.Database;

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
