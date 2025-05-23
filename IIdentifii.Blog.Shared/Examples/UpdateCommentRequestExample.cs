namespace IIdentifii.Blog.Shared
{
    public class UpdateCommentRequestExample : IExamplesProvider<UpdateCommentRequest>
    {
        public UpdateCommentRequest GetExamples() => new()
        {
            Id = SeedDataConstants.CommentId,
            Content = "Example Update Comment",
        };
    }
}
