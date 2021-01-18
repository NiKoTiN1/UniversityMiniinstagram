using System;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class CommentReport : IEntity
    {
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Comment Comment { get; set; }
        public string CommentId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
