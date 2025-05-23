namespace IIdentifii.Blog.Repository
{
    public class IIdentifiiUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = null!;

        public ICollection<BlogPostModel>? Posts { get; set; }

        public ICollection<ReactionModel>? Reactions { get; set; }
    }
}

