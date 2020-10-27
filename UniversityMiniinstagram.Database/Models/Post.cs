﻿    using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityMiniinstagram.Database
{
    public class Post
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Image Image { get; set; }
        public Guid ImageId { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        public Category category { get; set; }

        public string Description { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public bool IsShow { get; set; }
    }
}
