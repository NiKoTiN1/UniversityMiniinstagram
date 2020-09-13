using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityMiniinstagram.Database
{
    public class Image
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Path { get; set; }
        public DateTime UploadDate { get; set; }
        public Category category { get; set; }
    }
}
