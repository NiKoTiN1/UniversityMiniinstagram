namespace UniversityMiniinstagram.View
{
    public class EditProfileViewModel: ImageViewModel
    {
        public string Username { get; set; }
        public string Description { get; set; }
        public string WebRootPath { get; set; }
        public string Userid { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
