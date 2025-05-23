namespace IIdentifii.Blog.Shared
{
    public interface ISortable
    {
        /// <summary>
        /// Optional sorting criteria (e.g. by date, popularity).
        /// </summary>
        SortRequest? Sort { get; set; }
    }
}
