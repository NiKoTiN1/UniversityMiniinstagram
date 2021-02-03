using System;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class Image : IEntity
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Path { get; set; }

        public Post Post { get; set; }

    #nullable enable
        public string? PostId { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
