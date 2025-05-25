namespace IIdentifii.Blog.BusinessLogic
{
    public record ReactionOperation
    {
        public Guid BlogPostId { get; set; }

        public Guid UserId { get; set; }

        public ReactionOperationType OperationType { get; set; }

        public ReactionType Type { get; set; }

        public ReactionType? PreviousType { get; set; }

        public static ReactionOperation Create(
            ReactionModel reaction, 
            ReactionOperationType type, 
            ReactionType? previousType = null)
        {
            return new ReactionOperation
            {
                BlogPostId = reaction.BlogPostId,
                UserId = reaction.UserId,
                OperationType = type,
                Type = reaction.Type,
                PreviousType = previousType,
            };
        }
    }
}
