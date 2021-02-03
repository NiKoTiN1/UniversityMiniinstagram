using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class Post : IEntity
    {
        public Post() { }

        public Post(ILazyLoader lazyLoader)
        {
            this.lazyLoader = lazyLoader;
        }

        private readonly ILazyLoader lazyLoader;

        [Required]
        public string Id { get; set; }

        [Required]
        private Image image;

        public Image Image
        {
            get => this.lazyLoader.Load(this, ref this.image);
            set => this.image = value;
        }

        public string ImageId { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        public Category CategoryPost { get; set; }

        public string Description { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public ICollection<Like> Likes { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public PostReport Report { get; set; }

        public string ReportId { get; set; }

        public bool IsShow { get; set; }
    }
}
