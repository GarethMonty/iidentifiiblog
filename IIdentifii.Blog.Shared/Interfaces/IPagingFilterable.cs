namespace IIdentifii.Blog.Shared
{
    public interface IPagingFilterable : IFilterable
    {
        /// <summary>
        /// Optional paging options (page number, size, etc.).
        /// </summary>
        PagingRequest? Paging { get; set; }
    }
}
