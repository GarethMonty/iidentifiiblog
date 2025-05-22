namespace IIdentifii.Blog.Repository
{
    public class ReactionAggregateConfig : IEntityTypeConfiguration<ReactionAggregateModel>
    {
        public void Configure(
            EntityTypeBuilder<ReactionAggregateModel> builder)
        {
            builder.ToTable("ReactionAggregates");

            builder.HasKey(l => l.Id);

            builder.HasIndex(l => new { l.BlogPostId, l.Type })
                .IsUnique();

            builder.HasOne(l => l.BlogPost)
                .WithMany(p => p.ReactionAggregates)
                .HasForeignKey(l => l.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
