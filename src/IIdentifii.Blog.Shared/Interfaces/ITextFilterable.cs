namespace IIdentifii.Blog.Shared
{
    public interface ITextFilterable : IFilterable
    {
        /// <summary>
        /// Optional filtering criteria (e.g. keyword, tags).
        /// </summary>
        FilterTextRequest? Filter { get; set; }
    }
}
