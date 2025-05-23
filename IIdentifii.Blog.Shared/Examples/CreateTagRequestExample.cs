namespace IIdentifii.Blog.Shared
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples() => new()
        {
            BlogPostId = SeedDataConstants.BlogPostId,
            Type = TagType.Misleading,
        };
    }
}
