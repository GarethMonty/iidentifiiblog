namespace IIdentifii.Blog.Shared
{
    public class CreateBlogPostRequestExample : IExamplesProvider<CreateBlogPostRequest>
    {
        public CreateBlogPostRequest GetExamples() => new()
        {
            Title = "Example Title",
            Content = "Example Content",
        };
    }
}
