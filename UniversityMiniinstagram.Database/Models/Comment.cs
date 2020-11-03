using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityMiniinstagram.Database
{
    public class Comment
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
        public bool IsShow { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
