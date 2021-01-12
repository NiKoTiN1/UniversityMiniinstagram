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
        public DateTime UploadDate { get; set; }
    }
}
