using System.ComponentModel.DataAnnotations;
using UniversityMiniinstagram.Database.Interfaces;

namespace UniversityMiniinstagram.Database.Models
{
    public class Like : IEntity
    {
        [Required]
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public virtual Post Post { get; set; }

        public string PostId { get; set; }
    }
}
