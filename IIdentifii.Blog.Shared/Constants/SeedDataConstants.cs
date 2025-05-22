namespace IIdentifii.Blog.Shared
{
    public static class SeedDataConstants
    {
        public static Guid BlogPostId = Guid.Parse("8a267f6e-8b93-4274-b39c-5427263d2eb6");
        public static Guid CommentId = Guid.Parse("d5d404b1-5516-486c-8ae6-05cc3e124d50");
        public static Guid ReactionId = Guid.Parse("5dec58aa-d0cd-431b-b524-d42f5d7857e1");
        public static Guid TagId = Guid.Parse("7a055382-928b-4427-b9b1-58603a224b4c");

        public static Guid ModeratorId = Guid.Parse("bfe2743a-8bd7-431e-6b28-08dd991f81eb");
        public const string ModeratorEmail = "moderator@example.com";
        public const string ModeratorPassword = "Moderator123!";

        public static Guid UserId = Guid.Parse("b66734cc-8584-494c-6b29-08dd991f81eb");
        public const string UserEmail = "user@example.com";
        public const string UserPassword = "User123!";

    }
}
