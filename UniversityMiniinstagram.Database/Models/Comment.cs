using System;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class Comment : IEntity
    {
        public Comment() { }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Text { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public Post Post { get; set; }

        public string PostId { get; set; }

        public bool IsShow { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public CommentReport Report { get; set; }

        public string ReportId { get; set; }
    }
}
