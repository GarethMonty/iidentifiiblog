namespace IIdentifii.Blog.Shared
{
    public class CommentRequestExample : IExamplesProvider<CommentRequest>
    {
        public CommentRequest GetExamples() => new()
        {
            BlogPostId = SeedDataConstants.BlogPostId,
            Filter = new FilterTextRequest
            {
                Query = "Example Title"
            },
            DateFilter = new DateFilterRequest
            {
                From = DateTime.UtcNow.AddDays(-30),
                To = DateTime.UtcNow,
            },
            Paging = new PagingRequest
            {
                Page = 1,
                PageSize = 10,
            },
            UserId = SeedDataConstants.UserId,
        };
    }
}
