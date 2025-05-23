namespace IIdentifii.Blog.Shared
{
    public class BlogPostRequestExample : IExamplesProvider<BlogPostRequest>
    {
        public BlogPostRequest GetExamples() => new()
        {
            AuthorId = SeedDataConstants.UserId,
            DateFilter = new DateFilterRequest
            {
                From = DateTime.UtcNow.AddDays(-30),
                To = DateTime.UtcNow,
            },
            Filter = new FilterTextRequest
            {
                Query = ""
            },
            ReactionFilter = new ReactionFilterRequest
            {
                ExcludedTypes = new List<ReactionType> { ReactionType.Dislike, ReactionType.Love, ReactionType.Love },
                IncludeEntities = false,
            },
            TagFilter = new TagFilterRequest
            {
                Tags = new List<TagType> { TagType.Misleading },
            },
            Sort = new SortRequest
            {
                SortBy = SortByType.PostedAt,
                SortOrder = SortOrderType.Descending
            },
            Paging = new PagingRequest
            {
                Page = 1,
                PageSize = 10,
            }
        };
    }
}
