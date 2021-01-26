using System;

namespace UniversityMiniinstagram.Database.Constants
{
    [Flags]
    public enum Roles
    {
        Admin,
        Moderator,
        User,
        Banned
    }
}
