using UniversityMiniinstagram.Database.Models;

namespace UniversityMiniinstagram.Views
{
    public class CreatePostViewModel : ImageViewModel
    {
        public string Description { get; set; }
        public Category CategoryPost { get; set; }
    }
}
