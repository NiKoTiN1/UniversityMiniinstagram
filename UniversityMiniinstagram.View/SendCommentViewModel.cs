using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UniversityMiniinstagram.Database;

namespace UniversityMiniinstagram.View
{
    public class SendCommentViewModel
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
