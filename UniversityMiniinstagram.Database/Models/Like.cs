using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityMiniinstagram.Database
{
    public class Like
    {
        [Required]
        public Guid Id { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
    }
}
