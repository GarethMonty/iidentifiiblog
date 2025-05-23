namespace IIdentifii.Blog.Shared
{
    public class UpdateTagRequestExample : IExamplesProvider<UpdateTagRequest>
    {
        public UpdateTagRequest GetExamples() => new()
        {
            Id = SeedDataConstants.TagId,
            Type = TagType.Misleading,
        };
    }
}
