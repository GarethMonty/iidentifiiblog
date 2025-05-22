namespace IIdentifii.Blog.Shared
{
    public record ReactionOperation
    {
        public Guid BlogPostId { get; set; }

        public Guid UserId { get; set; }

        public ReactionOperationType OperationType { get; set; }

        public ReactionType Type { get; set; }

        public ReactionType? PreviousType { get; set; }
    }
}
