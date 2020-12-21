using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Views
{
    public class SendCommentViewModel
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}
