namespace IIdentifii.Blog.Shared
{
    /// <summary>
    /// A request object for filtering and retrieving blog posts.
    /// </summary>
    public record BlogPostRequest : IAuthorFilterable, IDateFilterable, ITextFilterable, IReactionFilterable, ITagFilterable, ISortable, IPagingFilterable
    {
        /// <summary>
        /// Filter by a specific author's ID.
        /// </summary>
        [JsonPropertyName("authorId")]
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Optional filter for date ranges (e.g. posted within a range).
        /// </summary>
        [JsonPropertyName("dateFilter")]
        public DateFilterRequest? DateFilter { get; set; }

        /// <summary>
        /// Optional filtering criteria (e.g. keyword, tags).
        /// </summary>
        [JsonPropertyName("filter")]
        public FilterTextRequest? Filter { get; set; }

        /// <summary>
        /// Optional filter for reactions (e.g. likes, dislikes).
        /// </summary>
        [JsonPropertyName("reactionFilter")]
        public ReactionFilterRequest? ReactionFilter { get; set; }

        /// <summary>
        /// Optional filter for tags
        /// </summary>
        [JsonPropertyName("tagFilter")]
        public TagFilterRequest? TagFilter { get; set; }

        [JsonPropertyName("sort")]
        public SortRequest? Sort { get; set; }

        /// <summary>
        /// Optional paging options (page number, size, etc.).
        /// </summary>
        [JsonPropertyName("paging")]
        public PagingRequest? Paging { get; set; }

        /// <summary>
        /// Executes validation logic on nested filters and paging.
        /// </summary>
        public void Validate()
        {
            DateFilter?.Validate();
            Filter?.Validate();
            ReactionFilter?.Validate();
            Paging?.Validate();
        }
    }
}
