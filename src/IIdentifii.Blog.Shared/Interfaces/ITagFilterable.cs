namespace IIdentifii.Blog.Shared
{
    public interface ITagFilterable : IFilterable
    {
        /// <summary>
        /// Optional filter for tags
        /// </summary>
        TagFilterRequest? TagFilter { get; set; }
    }
}
