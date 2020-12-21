using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.View
{
    public class CreatePostViewModel : ImageViewModel
    {
        public string Description { get; set; }
        public Category CategoryPost { get; set; }
    }
}
