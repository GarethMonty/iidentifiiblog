namespace IIdentifii.Blog.Shared
{
    public static class RoleConstants
    {
        public const string Moderator = "Moderator";
        public const string User = "User";
        public static List<string> AllRoles => new()
        {
            Moderator,
            User
        };
    }
}
