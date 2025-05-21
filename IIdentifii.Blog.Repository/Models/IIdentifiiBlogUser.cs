namespace IIdentifii.Blog.Shared
{
    public class IIdentifiiUser : IdentityUser<Guid>
    {
        public string DisplayName { get; set; } = null!;

        public string? Bio { get; set; }

        public ICollection<BlogPostModel>? Posts { get; set; }

        public ICollection<LikeModel>? Likes { get; set; }
    }
}

