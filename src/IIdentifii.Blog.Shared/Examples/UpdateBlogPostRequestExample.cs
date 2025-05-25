namespace IIdentifii.Blog.Shared
{
    public class UpdateBlogPostRequestExample : IExamplesProvider<UpdateBlogPostRequest>
    {
        public UpdateBlogPostRequest GetExamples() => new()
        {
            Id = SeedDataConstants.BlogPostId,
            Title = "Example Update Title",
            Content = "Example Update Content"
        };
    }
}
