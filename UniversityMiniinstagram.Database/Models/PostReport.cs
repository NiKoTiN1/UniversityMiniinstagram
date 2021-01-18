using System;
using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class PostReport : IEntity
    {
        public string Id { get; set; }
        public Post Post { get; set; }
        public string PostId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
