using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Database.Models
{
    public class Like
    {
        [Required]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
    }
}
