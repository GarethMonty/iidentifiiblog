namespace IIdentifii.Blog.Shared
{
    public interface IBlobPostFilterable : IFilterable
    {
        /// <summary>
        /// Filter by a specific blog post's ID.
        /// </summary>
        Guid? BlogPostId { get; set; }
    }
}
