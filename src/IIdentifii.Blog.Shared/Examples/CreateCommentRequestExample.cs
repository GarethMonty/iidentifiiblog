namespace IIdentifii.Blog.Shared
{
    public class CreateCommentRequestExample : IExamplesProvider<CreateCommentRequest>
    {
        public CreateCommentRequest GetExamples() => new()
        {
            BlogPostId = SeedDataConstants.BlogPostId,
            Content = "Example Comment",
        };
    }
}
