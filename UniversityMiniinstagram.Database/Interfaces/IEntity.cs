using System.ComponentModel.DataAnnotations;

namespace UniversityMiniinstagram.Database.Interfaces
{
    public interface IEntity
    {
        [Required]
        public string Id { get; set; }
    }
}
