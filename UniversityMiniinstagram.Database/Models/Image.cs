using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Database.Models
{
    public class Image
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
