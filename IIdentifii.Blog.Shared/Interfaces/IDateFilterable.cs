namespace IIdentifii.Blog.Shared
{
    public interface IDateFilterable : IFilterable
    {
        /// <summary>
        /// Optional filter for date ranges (e.g. posted within a range).
        /// </summary>
        DateFilterRequest? DateFilter { get; set; }
    }
}
